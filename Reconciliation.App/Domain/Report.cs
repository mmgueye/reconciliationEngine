namespace Reconciliation.App.Domain
{
    
    public class Report
    {

        public List<Matching> Matches { get; set; } = new();
        public List<Transaction> UnmatchedBank { get; set; } = new();
        public List<Transaction> UnmatchedAccounting { get; set; } = new();
        public List<Matching> AmbiguousMatches { get; set; } = new();

        
    }
}