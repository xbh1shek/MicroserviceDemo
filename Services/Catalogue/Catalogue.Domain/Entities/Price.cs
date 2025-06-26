
using Catalogue.Domain.Abstractions;
namespace Catalogue.Domain.Entities;
public record Price : ValueObject
{
    public decimal Amount { get; }
    public string? Currency { get; }
    private Price() { }

    public Price(decimal amount, string currency)
    {
        Amount = amount.NotNegativeOrZero();
        Currency = currency.NotNullOrWhiteSpace(nameof(currency));
    }
   
}

