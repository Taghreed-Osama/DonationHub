using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DonationHub.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private static readonly Regex EmailRegex = new("^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public string Value { get; set; }

        private Email(string value = "") => Value = value;

        public static Email Create(string valve)
        {
            if (string.IsNullOrWhiteSpace(valve))
            {
                throw new ArgumentException("Email is required.", nameof(valve));
            }

            var normalized = valve.Trim().ToLowerInvariant();
            if (!EmailRegex.IsMatch(valve))
            {
                throw new ArgumentException("Your email is not invalid.", nameof(valve));
            }

            return new Email(normalized);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
