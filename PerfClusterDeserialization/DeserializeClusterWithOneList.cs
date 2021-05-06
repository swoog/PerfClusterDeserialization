using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PerfClusterDeserialization
{
    public class DeserializeClusterWithOneList
    {
        public static async Task<List<BaseItem>> Deserialize(string fileName)
        {
            await using FileStream createStream = File.OpenRead(fileName);
            return await JsonSerializer.DeserializeAsync<List<BaseItem>>(createStream,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters =
                    {
                        new JsonStringEnumConverter(),
                        new ClusterConverter()
                    }
                });
        }
    }
}