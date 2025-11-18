using Xunit;
using Moq;
using FinancialCalculator.Core;

public class CalculatorTests
{
    private readonly CreditCalculator credit = new();
    private readonly DepositCalculator deposit = new();
    private readonly CurrencyConverter currency = new();

    [Fact]
    public void Credit_100k_12m_10p_Correct()
    {
        var result = credit.Calculate(100000, 12, 10);
        Assert.Equal(8791.59, result.MonthlyPayment, 2);
        Assert.Equal(5499.08, result.Overpayment, 2);
    }

    [Theory]
    [InlineData(500000, 24, 0, 20833.33)]
    [InlineData(100000, 12, 0, 8333.33)]
    public void Credit_ZeroRate_Works(double amount, int months, double rate, double expected)
    {
        var result = credit.Calculate(amount, months, rate);
        Assert.Equal(expected, result.MonthlyPayment, 2);
    }

    [Fact]
    public void Currency_RUB_to_USD()
    {
        Assert.Equal(100.0, currency.Convert("RUB", "USD", 9000), 2);
    }

    [Theory]
    [InlineData("USD", "RUB", 100, 9000)]
    [InlineData("EUR", "RUB", 100, 9850)]
    public void Currency_VariousPairs(string from, string to, double amount, double expected)
    {
        Assert.Equal(expected, currency.Convert(from, to, amount), 2);
    }

    [Fact]
    public void Deposit_WithCapitalization_616778()
    {
        var result = deposit.Calculate(100000, 12, 6, true);
        Assert.Equal(6167.78, result.Income, 2);
        Assert.Equal(106167.78, result.Total, 2);
    }


    [Fact]
    public void Deposit_WithoutCapitalization_6000()
    {
        var result = deposit.Calculate(100000, 12, 6, false);
        Assert.Equal(6000.0, result.Income, 2);
    }

    [Fact]
    public void Currency_UnsupportedPair_LogsError()
    {
        var mockLogger = new Mock<ILogger>();
        var converter = new LoggingCurrencyConverter(currency, mockLogger.Object);

        Assert.Throws<ArgumentException>(() => converter.Convert("BTC", "USD", 100));

        mockLogger.Verify(x => x.Log(It.IsAny<string>()), Times.Once);
    }
}

public interface ILogger
{
    void Log(string message);
}

public class LoggingCurrencyConverter
{
    private readonly CurrencyConverter _converter;
    private readonly ILogger _logger;

    public LoggingCurrencyConverter(CurrencyConverter converter, ILogger logger)
    {
        _converter = converter;
        _logger = logger;
    }

    public double Convert(string from, string to, double amount)
    {
        try
        {
            return _converter.Convert(from, to, amount);
        }
        catch (ArgumentException ex)
        {
            _logger.Log($"[ERROR] {ex.Message}");
            throw;
        }
    }
}