namespace Reconciliation.App.Domain
{

    public enum MatchRule
    {
        PerfectMatch,             // score 100
        DateTolerance,            // score 85
        AmountTolerance,          // score 70
        DateAndAmountTolerance    // score 55
    }

    public class Matching
    {
        public int BankId { get; set; }
        public int AccountingId { get; set; }
        public int Score { get; set; }
        public MatchRule RuleApplied { get; set; }
        public bool IsAmbiguous { get; set; }
    }
}