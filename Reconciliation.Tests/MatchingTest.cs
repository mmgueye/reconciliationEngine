using Reconciliation.App.Services;
using Reconciliation.App.Domain;

namespace Reconciliation.Tests
{
    [TestClass]
    public class MatchingTest
    {

        [TestMethod]
        public void Match_Exact_ShouldScore100()
        {
            var bank = new List<Transaction>
            {
                new Transaction { Id = 1, Date = new DateTime(2023,10,1), Amount = -50 }
            };

            var account= new List<Transaction>
            {
                new Transaction { Id = 10, Date = new DateTime(2023,10,1), Amount = -50 }
            };
            var logger = new Logger("test_logs.txt");

            var engine = new ReconciliationEngine(logger);
            var report = engine.Match(bank, account);

            Assert.AreEqual(1, report.Matches.Count);
            Assert.AreEqual(100, report.Matches[0].Score);
            Assert.AreEqual(MatchRule.PerfectMatch, report.Matches[0].RuleApplied);
        }

        [TestMethod]
        public void Match_SameAmount_DateTolerance_ShouldScore85()
        {
            var bank = new List<Transaction>
            {
                new Transaction { Id = 1, Date = new DateTime(2023,10,2), Amount = -50 }
            };

            var account= new List<Transaction>
            {
                new Transaction { Id = 10, Date = new DateTime(2023,10,1), Amount = -50 }
            };
            var logger = new Logger("test_logs.txt");
            var engine = new ReconciliationEngine(logger);
            var report = engine.Match(bank, account);

            Assert.AreEqual(85, report.Matches[0].Score);
            Assert.AreEqual(MatchRule.DateTolerance, report.Matches[0].RuleApplied);
        }

        [TestMethod]
        public void Match_Ambiguous_ShouldBeFlagged()
        {
            var bank = new List<Transaction>
            {
                new Transaction { Id = 1, Date = new DateTime(2023,10,1), Amount = -50 }
            };

            var account= new List<Transaction>
            {
                new Transaction { Id = 10, Date = new DateTime(2023,10,1), Amount = -50 },
                new Transaction { Id = 11, Date = new DateTime(2023,10,1), Amount = -50 }
            };
            var logger = new Logger("test_logs.txt");
            var engine = new ReconciliationEngine(logger);
            var report = engine.Match(bank, account);

            Assert.IsTrue(report.Matches[0].IsAmbiguous);
        }

    }

}