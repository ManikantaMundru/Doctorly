using Doctorly.Calendar.Domain.Common;
using System.Text.RegularExpressions;

namespace Doctorly.Calendar.Domain.ValueObjects
{
    public sealed class EmailAddress
    {
        public string Value { get; }

        private EmailAddress(string value)
        {
            Value = value;
        }

        public static EmailAddress Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Email is required.");

            var normalized = value.Trim().ToLowerInvariant();

            if (normalized.Length > 256)
                throw new DomainException("Email must not exceed 256 characters.");

            var isValid = Regex.IsMatch(normalized, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!isValid)
                throw new DomainException("Email format is invalid.");

            return new EmailAddress(normalized);
        }

        public override string ToString() => Value;
    }
}
