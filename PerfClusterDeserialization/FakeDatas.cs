using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PerfClusterDeserialization
{
    public class FakeDatas
    {
        public static void GenerateTwoList(string fileName, List<BaseItem> datas)
        {
            var twoListData = new TwoListData
            {
                Clusters = datas.Where(d => d.Mode == ItemMode.Cluster).Cast<Cluster>().ToList(),
                Places = datas.Where(d => d.Mode == ItemMode.Place).Cast<Place>().ToList()
            };

            using FileStream createStream = File.Create(fileName);
            using var utf8JsonWriter = new Utf8JsonWriter(createStream);
            JsonSerializer.Serialize(utf8JsonWriter,
                twoListData,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters =
                    {
                        new JsonStringEnumConverter()
                    }
                });
            
            utf8JsonWriter.Flush();
            createStream.Flush();
        }

        public static void GenerateOneList(string fileName, List<BaseItem> datas)
        {
            using var createStream = File.Create(fileName);
            using var utf8JsonWriter = new Utf8JsonWriter(createStream);
            JsonSerializer.Serialize(utf8JsonWriter,
                datas,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters =
                    {
                        new JsonStringEnumConverter(),
                        new ClusterConverter()
                    }
                });
            utf8JsonWriter.Flush();
            createStream.Flush();
        }

        private static Random _random = new Random();

        public static List<BaseItem> GenerateDatas(int n)
        {
            return Enumerable.Range(0, n)
                .Select(i => GenerateData())
                .ToList();
        }

        private static BaseItem GenerateData()
        {
            var val = _random.Next(0, 100);

            if (val > 50)
            {
                return new Cluster()
                {
                    Mode = ItemMode.Cluster,
                    Count = _random.Next(0, 200),
                    Attributes = new[]
                    {
                        "Att1",
                        "Att2",
                        "Att3"
                    }
                };
            }

            return new Place()
            {
                Mode = ItemMode.Place,
                Name = "Place " + _random.Next(0, 200),
            };
        }
    }
}