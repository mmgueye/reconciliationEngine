using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reconciliation.App.Services;
using Reconciliation.App.Domain;

namespace Reconciliation.Tests
{
    [TestClass]
    public class ParsingTests
    {
        [TestMethod]
        public void Parse_ValidFile_ShouldReturnOneTransaction()
        {
            string filePath = "test_valid.csv";
            File.WriteAllLines(filePath, new[]
            {
                "Date,Description,Amount",
                "2023-10-01,Test,-10.50"
            });

            string errorFile = "errors_test.txt";
            if (File.Exists(errorFile)) File.Delete(errorFile);

            var parser = new Parser(errorFile);

            var result = parser.Parse(filePath, TransactionType.Bank);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(-10.50m, result[0].Amount);
            Assert.AreEqual(TransactionType.Bank, result[0].Type);
        }

        [TestMethod]
        public void Parse_InvalidLine_ShouldSkipAndLogError()
        {
            string filePath = "test_invalid.csv";
            File.WriteAllText(filePath,
            @"Date,Description,Amount
            INVALID_DATE,Test,-10.50");

            string errorFile = "errors_test.txt";
            if (File.Exists(errorFile)) File.Delete(errorFile);

            var parser = new Parser(errorFile);

            var result = parser.Parse(filePath, TransactionType.Bank);

            Assert.AreEqual(0, result.Count);
            Assert.IsTrue(File.Exists(errorFile));
        }
    }
}