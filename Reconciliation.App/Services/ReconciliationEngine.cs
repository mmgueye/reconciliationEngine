using Reconciliation.App.Domain;

namespace Reconciliation.App.Services
{
    public class ReconciliationEngine
    {
         private readonly Logger _logger;

    public ReconciliationEngine(Logger logger)
    {
        _logger = logger;
    }

        public Report Match(List<Transaction> bankTransactions, List<Transaction> accountingTransactions)
        {
            _logger.Info($"Début du matching : {bankTransactions.Count} banques, {accountingTransactions.Count} compta");

            var report = new Report();
            var usedAccounting = new HashSet<int>();

            foreach (var bank in bankTransactions)
            {
                var candidates = new List<(Transaction transaction, int score, MatchRule rule, int dateDiff, decimal amountDiff)>();

                foreach (var accounting in accountingTransactions.Where(a => !usedAccounting.Contains(a.Id)))
                {
                    var match = TryMatch(bank, accounting);
                    if (match != null)
                    {
                        candidates.Add(match.Value);
                    }
                }

                if (candidates.Count == 0)
                {
                    _logger.Warn($"Transaction bancaire {bank.Id} non matchée");
                    continue;
                }

                var ordered = candidates
                    .OrderByDescending(c => c.score)
                    .ThenBy(c => c.dateDiff)
                    .ThenBy(c => c.amountDiff)
                    .ThenBy(c => c.transaction.Id)
                    .ToList();

                var best = ordered.First();

                bool ambiguous = ordered.Count > 1 &&
                                 ordered[1].score == best.score &&
                                 ordered[1].dateDiff == best.dateDiff &&
                                 ordered[1].amountDiff == best.amountDiff;

                report.Matches.Add(new Matching
                {
                    BankId = bank.Id,
                    AccountingId = best.transaction.Id,
                    Score = best.score,
                    RuleApplied = best.rule,
                    IsAmbiguous = ambiguous
                });

                _logger.Info($"Match Banque {bank.Id} → Compta {best.transaction.Id}, Score {best.score}, Ambigu: {ambiguous}");

                usedAccounting.Add(best.transaction.Id);
            }
            report.UnmatchedBank = bankTransactions
            .Where(b => !report.Matches.Any(m => m.BankId == b.Id))
            .ToList();

            report.UnmatchedAccounting = accountingTransactions
                .Where(a => !report.Matches.Any(m => m.AccountingId == a.Id))
                .ToList();
       
            _logger.Info($"Fin du matching : {report.Matches.Count} matches, {report.UnmatchedBank.Count} non matchées banques, {report.UnmatchedAccounting.Count} non matchées compta");

            return report;
        }

        private (Transaction transaction, int score, MatchRule rule, int dateDiff, decimal amountDiff)? TryMatch(
            Transaction bank, Transaction accounting)
        {
            int dateDiff = Math.Abs((bank.Date - accounting.Date).Days);
            decimal amountDiff = Math.Abs(bank.Amount - accounting.Amount);

            if (bank.Date == accounting.Date && bank.Amount == accounting.Amount)
                return (accounting, 100, MatchRule.PerfectMatch, dateDiff, amountDiff);

            if (bank.Amount == accounting.Amount && dateDiff <= 1)
                return (accounting, 85, MatchRule.DateTolerance, dateDiff, amountDiff);

            if (bank.Date == accounting.Date && amountDiff <= 5)
                return (accounting, 70, MatchRule.AmountTolerance, dateDiff, amountDiff);

            if (dateDiff <= 2 && amountDiff <= 5)
                return (accounting, 55, MatchRule.DateAndAmountTolerance, dateDiff, amountDiff);

            return null;
        }
    }
}