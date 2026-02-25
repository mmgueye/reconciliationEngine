using System.Text;
using Reconciliation.App.Domain;

namespace Reconciliation.App.Services
{
    public class ReportExporter
    {
        public void Export(Report report)
        {
            ExportMatchesCsv(report);
            ExportSummaryReport(report);
        }

        private void ExportMatchesCsv(Report report)
        {
            var txt = new StringBuilder();
            txt.AppendLine("BankId,AccountingId,Score,RuleApplied");

            foreach (var match in report.Matches)
            {
                txt.AppendLine($"{match.BankId},{match.AccountingId},{match.Score},{match.RuleApplied}");
            }

            File.WriteAllText("matches.csv", txt.ToString());
        }

        private void ExportSummaryReport(Report report)
        {
            var txt = new StringBuilder();

            txt.AppendLine($"Nb total banque: {report.Matches.Count + report.UnmatchedBank.Count}");
            txt.AppendLine($"Nb total compta: {report.Matches.Count + report.UnmatchedAccounting.Count}");
            txt.AppendLine($"Nb matchés: {report.Matches.Count}");
            txt.AppendLine($"Nb non matchés banque: {report.UnmatchedBank.Count}");
            txt.AppendLine($"Nb non matchés compta: {report.UnmatchedAccounting.Count}");

            int weakMatches = report.Matches.Count(m => m.Score < 85);
            txt.AppendLine($"Nb matchs faibles (score < 85): {weakMatches}");

            txt.AppendLine();
            txt.AppendLine("Cas ambigus:");

            foreach (var match in report.Matches.Where(m => m.IsAmbiguous))
            {
                txt.AppendLine($"BankId {match.BankId} -> AccountingId {match.AccountingId}");
            }

            File.WriteAllText("report.txt", txt.ToString());
        }
    }
}