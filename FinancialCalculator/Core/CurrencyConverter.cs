using System.Collections.Generic;

namespace FinancialCalculator.Core
{
    public class CurrencyConverter
    {
        private readonly Dictionary<(string From, string To), double> _rates;

        public CurrencyConverter()
        {
            _rates = new()
            {
                { ("RUB", "USD"), 1 / 90.0 },
                { ("RUB", "EUR"), 1 / 98.5 },
                { ("USD", "RUB"), 90.0 },
                { ("USD", "EUR"), 90.0 / 98.5 },
                { ("EUR", "RUB"), 98.5 },
                { ("EUR", "USD"), 98.5 / 90.0 }
            };
        }

        public double Convert(string from, string to, double amount)
        {
            from = from.ToUpper();
            to = to.ToUpper();

            if (from == to) return Math.Round(amount, 2);

            if (_rates.TryGetValue((from, to), out double rate))
                return Math.Round(amount * rate, 2);

            throw new ArgumentException("Неподдерживаемая валюта");
        }
    }
}