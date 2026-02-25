using System.Globalization;
using Reconciliation.App.Domain;

namespace Reconciliation.App.Services
{
    public class Parser
    {
        private readonly string _errorFilePath;

        public Parser(string errorFilePath)
        {
            _errorFilePath = errorFilePath;
        }

        public List<Transaction> Parse(string filePath, TransactionType type)
        {
            var transactions = new List<Transaction>();

             if (!File.Exists(filePath))
            {
                LogError($"File not found: {filePath}");
                return transactions;
            }

            var lines = File.ReadAllLines(filePath);

            if (lines.Length == 0)
            {
                LogError($"Empty file: {filePath}");
                return transactions;
            }

            int idCounter = 1;

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var columns = line.Split(',');

                if (columns.Length != 3)
                {
                    LogError($"Invalid column count line {i + 1}: {line}");
                    continue;
                }

                if (!DateTime.TryParseExact(
                        columns[0],
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime date))
                {
                    LogError($"Invalid date line {i + 1}: {line}");
                    continue;
                }

                if (!decimal.TryParse(
                        columns[2],
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out decimal amount))
                {
                    LogError($"Invalid amount line {i + 1}: {line}");
                    continue;
                }

                var transaction = new Transaction
                {
                    Id = idCounter++,
                    Date = date,
                    Description = columns[1],
                    Amount = amount,
                    Type = type
                };

                transactions.Add(transaction);
            }

            return transactions;
        }

        private void LogError(string message)
        {
            File.AppendAllText(_errorFilePath, message + Environment.NewLine);
        }
    }
   
}