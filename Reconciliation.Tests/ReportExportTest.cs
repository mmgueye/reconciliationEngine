using Reconciliation.App.Domain;
using Reconciliation.App.Services;


namespace Reconciliation.Tests
{
    [TestClass]
    public class ReportExporterTests
    {
        [TestMethod]
        public void Export_ShouldCreateFiles()
        {
            var report = new Report
            {
                Matches = new List<Matching>
                {
                    new Matching
                    {
                        BankId = 1,
                        AccountingId = 10,
                        Score = 100,
                        RuleApplied = MatchRule.PerfectMatch,
                        IsAmbiguous = false
                    }
                },
                UnmatchedBank = new List<Transaction>(),
                UnmatchedAccounting = new List<Transaction>()
            };

            var exporter = new ReportExporter();

            if (File.Exists("matches.csv")) File.Delete("matches.csv");
            if (File.Exists("report.txt")) File.Delete("report.txt");

            // Act
            exporter.Export(report);

            // Assert
            Assert.IsTrue(File.Exists("matches.csv"));
            Assert.IsTrue(File.Exists("report.txt"));

            string matchesContent = File.ReadAllText("matches.csv");
            Assert.IsTrue(matchesContent.Contains("BankId,AccountingId,Score,RuleApplied"));
            Assert.IsTrue(matchesContent.Contains("1,10,100,PerfectMatch"));

            string reportContent = File.ReadAllText("report.txt");
            Assert.IsTrue(reportContent.Contains("Nb matchés: 1"));
        }
    }
}