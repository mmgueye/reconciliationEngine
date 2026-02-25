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

            var acc = new List<Transaction>
            {
                new Transaction { Id = 10, Date = new DateTime(2023,10,1), Amount = -50 }
            };

            var engine = new ReconciliationEngine();
            var result = engine.Match(bank, acc);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(100, result[0].Score);
            Assert.AreEqual(MatchRule.PerfectMatch, result[0].RuleApplied);
        }

        [TestMethod]
        public void Match_SameAmount_DateTolerance_ShouldScore85()
        {
            var bank = new List<Transaction>
            {
                new Transaction { Id = 1, Date = new DateTime(2023,10,2), Amount = -50 }
            };

            var acc = new List<Transaction>
            {
                new Transaction { Id = 10, Date = new DateTime(2023,10,1), Amount = -50 }
            };

            var engine = new ReconciliationEngine();
            var result = engine.Match(bank, acc);

            Assert.AreEqual(85, result[0].Score);
            Assert.AreEqual(MatchRule.DateTolerance, result[0].RuleApplied);
        }

        [TestMethod]
        public void Match_Ambiguous_ShouldBeFlagged()
        {
            var bank = new List<Transaction>
            {
                new Transaction { Id = 1, Date = new DateTime(2023,10,1), Amount = -50 }
            };

            var acc = new List<Transaction>
            {
                new Transaction { Id = 10, Date = new DateTime(2023,10,1), Amount = -50 },
                new Transaction { Id = 11, Date = new DateTime(2023,10,1), Amount = -50 }
            };

            var engine = new ReconciliationEngine();
            var result = engine.Match(bank, acc);

            Assert.IsTrue(result[0].IsAmbiguous);
        }

    }

}