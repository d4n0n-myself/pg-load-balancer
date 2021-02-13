namespace LoadBalancer.Domain.Models
{
    public class QueryPlan
    {
        public string Unformatted { get; set; } // todo mb not required
        public string NodeType { get; set; }
        public string RelationName { get; set; }
        public string Alias { get; set; }
        
        public float StartupCost { get; set; }
        public float TotalCost { get; set; }
        public long Rows { get; set; }
        public int Width { get; set; }
    }
}