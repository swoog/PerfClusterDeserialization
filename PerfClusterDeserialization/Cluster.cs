namespace PerfClusterDeserialization
{
    public class Cluster : BaseItem
    {
        public override ItemMode Mode { get; set; }
        public int Count { get; set; }

        public string[] Attributes { get; set; }
    }
}