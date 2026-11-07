using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.ValueObjects
{
    public sealed class Money : ValueObject
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Currency is required", nameof(currency));
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("This amount is invalid", nameof(amount));
            }

            currency = currency.Trim().ToUpperInvariant();

            return new Money(amount, currency);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
