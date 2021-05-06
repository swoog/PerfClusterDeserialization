using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PerfClusterDeserialization
{
    internal class ClusterConverter : JsonConverter<BaseItem>
    {
        public override BaseItem? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            if (!reader.Read() 
                || reader.TokenType != JsonTokenType.PropertyName
                || reader.GetString() != "mode")
            {
                throw new JsonException();
            }
            
            if (!reader.Read() 
                || reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            if (!Enum.TryParse<ItemMode>(reader.GetString(), true, out var mode))
            {
                throw new JsonException();
            }
            
            return mode switch
            {
                ItemMode.Cluster => GetCluster(ref reader, options),
                ItemMode.Place => GetPlace(ref reader),
                _ => throw new JsonException()
            }; 
        }

        private static Place GetPlace(ref Utf8JsonReader reader)
        {
            var place = new Place();
            place.Mode = ItemMode.Place;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                switch (reader.GetString())
                {
                    case "name":
                        if (!reader.Read() || reader.TokenType != JsonTokenType.String)
                        {
                            throw new JsonException();
                        }

                        place.Name = reader.GetString();
                        break;
                    default:
                        throw new JsonException();
                }
            }

            return place;
        }
        
        private static Cluster GetCluster(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var cluster = new Cluster();
            cluster.Mode = ItemMode.Cluster;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                switch (reader.GetString())
                {
                    case "count":
                        if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
                        {
                            throw new JsonException();
                        }

                        cluster.Count = reader.GetInt32();
                        break;
                    case "attributes":
                        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray)
                        {
                            throw new JsonException();
                        }

                        cluster.Attributes = JsonSerializer.Deserialize<string[]>(ref reader, options);
                        break;
                    default:
                        throw new JsonException();
                }
            }

            return cluster;
        }

        public override void Write(Utf8JsonWriter writer, BaseItem value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}