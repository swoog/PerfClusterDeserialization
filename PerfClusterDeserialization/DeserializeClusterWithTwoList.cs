using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PerfClusterDeserialization
{
    public class DeserializeClusterWithTwoList
    {
        public static async Task<List<BaseItem>> Deserialize(string fileName)
        {
            await using var createStream = File.OpenRead(fileName);
            var datas = await JsonSerializer.DeserializeAsync<TwoListData>(createStream,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters =
                    {
                        new JsonStringEnumConverter(),
                    }
                });

            return datas.Clusters.Union<BaseItem>(datas.Places).ToList();
        }
    }
}