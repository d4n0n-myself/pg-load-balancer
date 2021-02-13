namespace LoadBalancer.Domain.Models
{
    public class Query
    {
        public string Text { get; set; }
        public StatementType Type { get; set; }

        // if type = select 
        public bool HasCte { get; set; }
        public bool HasSelectionFromViews { get; set; }
        public bool HasComplexTypes { get; set; } // ???
        public bool HasSubqueries { get; set; }
    }
}