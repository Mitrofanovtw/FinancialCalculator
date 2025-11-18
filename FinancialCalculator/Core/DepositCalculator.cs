public class DepositCalculator
{
    public (double Income, double Total) Calculate(double amount, int months, double rate, bool capitalization)
    {
        if (amount <= 0 || months <= 0 || rate < 0)
            throw new ArgumentException();

        if (!capitalization)
        {
            double income = amount * rate / 100.0 * months / 12.0;
            return (Math.Round(income, 2), Math.Round(amount + income, 2));
        }

        double monthlyRate = rate / 1200.0;
        double total = amount * Math.Pow(1 + monthlyRate, months);
        total = Math.Round(total, 2);
        double finalIncome = total - amount;

        return (Math.Round(finalIncome, 2), total);
    }
}
