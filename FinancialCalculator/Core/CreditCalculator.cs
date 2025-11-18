using System;

public class CreditCalculator
{
    public (double MonthlyPayment, double Total, double Overpayment) Calculate(double amount, int months, double rate)
    {
        if (amount <= 0 || months <= 0 || rate < 0)
            throw new ArgumentException("Некорректные параметры");

        if (rate == 0)
            return (Math.Round(amount / months, 2), amount, 0);

        double r = rate / 12 / 100.0;
        double power = Math.Pow(1 + r, months);
        double payment = amount * (r * power) / (power - 1);

        double monthlyRounded = Math.Round(payment, 2);
        double total = monthlyRounded * months;
        double overpayment = total - amount;

        return (monthlyRounded, Math.Round(total, 2), Math.Round(overpayment, 2));
    }
}