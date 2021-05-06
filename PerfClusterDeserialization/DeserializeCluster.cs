using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerfClusterDeserialization
{
    [ShortRunJob]
    [ShortRunJob]
    [MemoryDiagnoser]
    [RPlotExporter]
    public class DeserializeCluster
    {
        [Params(10, 100, 1000, 2000, 5000, 7000, 10000)] public int N;
        private string twoListFileName;
        private string oneListfileName;

        [GlobalSetup]
        public void Setup()
        {
            var time = DateTime.Now.Ticks;
            this.twoListFileName = $"two-list-{time}.json";
            this.oneListfileName = $"one-list-{time}.json";

            var data = FakeDatas.GenerateDatas(this.N);
            FakeDatas.GenerateOneList(this.oneListfileName, data);
            FakeDatas.GenerateTwoList(this.twoListFileName, data);
        }

        [Benchmark]
        public Task<List<BaseItem>> OneList() => DeserializeClusterWithOneList.Deserialize(this.oneListfileName);

        [Benchmark]
        public Task<List<BaseItem>> TwoList() => DeserializeClusterWithTwoList.Deserialize(this.twoListFileName);
    }
}