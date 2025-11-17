using System;
using System.Collections.Generic;

namespace FinancialCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Финансовый калькулятор ";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("  ФИНАНСОВЫЙ КАЛЬКУЛЯТОР   ");
                Console.WriteLine("1. Расчет кредита");
                Console.WriteLine("2. Конвертер валют");
                Console.WriteLine("3. Калькулятор вкладов");
                Console.WriteLine("4. Выход\n");
                Console.Write("Выберите опцию (1-4): ");

                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": CreditCalculator.Run(); break;
                    case "2": CurrencyConverter.Run(); break;
                    case "3": DepositCalculator.Run(); break;
                    case "4":
                        Console.WriteLine("\nСпасибо за использование");
                        return;
                    default:
                        ShowError("Выберите цифру от 1 до 4");
                        break;
                }
            }
        }
        public static void ShowError(string msg)
        {
            Console.WriteLine($"\nОшибка: {msg}");
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }

        public static void Wait()
        {
            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        public static double ReadPositiveDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double value) && value > 0)
                    return value;
                Console.WriteLine("Ошибка. Введите положительное число.");
            }
        }

        public static int ReadPositiveInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
                    return value;
                Console.WriteLine("Ошибка. Введите целое положительное число.");
            }
        }
    }
    public static class CreditCalculator
    {
        public static void Run()
        {
            Console.Clear();
            Console.WriteLine("=== РАСЧЁТ КРЕДИТА ===\n");

            double sum = Program.ReadPositiveDouble("Сумма кредита (руб): ");
            int months = Program.ReadPositiveInt("Срок кредита (месяцев): ");
            double rate = Program.ReadPositiveDouble("Процентная ставка (% годовых): ");

            double monthlyRate = rate / 12 / 100;
            double payment = sum * monthlyRate / (1 - Math.Pow(1 + monthlyRate, -months));
            double total = payment * months;
            double overpayment = total - sum;

            Console.WriteLine($"\nЕжемесячный платёж:   {payment:F2} руб");
            Console.WriteLine($"Общая сумма выплат:   {total:F2} руб");
            Console.WriteLine($"Переплата:            {overpayment:F2} руб");

            Program.Wait();
        }
    }
    public static class CurrencyConverter
    {
        private static readonly Dictionary<(string from, string to), double> Rates = new()
        {
            { ("RUB", "USD"), 1 / 81.13 },
            { ("RUB", "EUR"), 1 / 94.42 },
            { ("USD", "RUB"), 81.13 },
            { ("USD", "EUR"), 1 / 1.16 },
            { ("EUR", "RUB"), 94.42 },
            { ("EUR", "USD"), 0.86 }
        };

        public static void Run()
        {
            Console.Clear();
            Console.WriteLine("=== КОНВЕРТЕР ВАЛЮТ ===\n");
            Console.WriteLine("Фиксированные курсы:");
            Console.WriteLine("  USD → RUB: 81.13");
            Console.WriteLine("  EUR → RUB: 94.42");
            Console.WriteLine("  EUR → USD: 1.16\n");

            string from = ReadCurrency("Исходная валюта (RUB/USD/EUR): ");
            string to = ReadCurrency("Целевая валюта (RUB/USD/EUR): ");
            double amount = Program.ReadPositiveDouble("Сумма: ");

            if (from == to)
            {
                Console.WriteLine($"\nРезультат: {amount:F2} {to}");
            }
            else if (Rates.TryGetValue((from, to), out double rate))
            {
                double result = amount * rate;
                Console.WriteLine($"\n{amount:F2} {from} → {result:F2} {to}");
            }
            else
            {
                Program.ShowError("Конвертация между этими валютами не поддерживается.");
            }

            Program.Wait();
        }

        private static string ReadCurrency(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim().ToUpper();
                if (input is "RUB" or "USD" or "EUR") return input;
                Console.WriteLine("Ошибка. Доступны только RUB, USD, EUR.");
            }
        }
    }
    public static class DepositCalculator
    {
        public static void Run()
        {
            Console.Clear();
            Console.WriteLine("=== КАЛЬКУЛЯТОР ВКЛАДОВ ===\n");

            double sum = Program.ReadPositiveDouble("Сумма вклада (руб): ");
            int months = Program.ReadPositiveInt("Срок вклада (месяцев): ");
            double rate = Program.ReadPositiveDouble("Процентная ставка (% годовых): ");

            Console.Write("Тип вклада (1 — с капитализацией, 2 — без): ");
            bool capitalization = Console.ReadLine() == "1";

            double income, total;
            if (capitalization)
            {
                total = sum * Math.Pow(1 + rate / 100 / 12, months);
                income = total - sum;
            }
            else
            {
                income = sum * rate / 100 * months / 12;
                total = sum + income;
            }

            Console.WriteLine($"\nДоход по вкладу:   {income:F2} руб");
            Console.WriteLine($"Итоговая сумма:    {total:F2} руб");

            Program.Wait();
        }
    }
}