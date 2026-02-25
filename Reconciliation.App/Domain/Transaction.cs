namespace Reconciliation.App.Domain
{

    public enum TransactionType
    {
        Bank,
        Account
    }

    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
    }
}