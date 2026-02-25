using Reconciliation.App.Services;
using Reconciliation.App.Domain;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Reconciliation Engine ===");

        if (args.Length < 2)
        {
            Console.WriteLine("Not enough Documents");
            return;
        }

        string bankFile = args[0];
        string accountingFile = args[1];
        string errorFile = "errors.txt";

        if (!File.Exists(bankFile) || !File.Exists(accountingFile))
        {
            Console.WriteLine("Error: one or two input document do not exist");
            return;
        }

        try
        {
            var parser = new Parser(errorFile);
            var bankTransactions = parser.Parse(bankFile, TransactionType.Bank);
            var accountingTransactions = parser.Parse(accountingFile, TransactionType.Account);

            var engine = new ReconciliationEngine();
            var report = engine.Match(bankTransactions, accountingTransactions);

            var exporter = new ReportExporter();
            exporter.Export(report);

            Console.WriteLine("Reconciliation succes");
 
           
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fatal error: " + ex.Message);
        }
    }
}