using Doctorly.Calendar.Domain.Common;
using Doctorly.Calendar.Domain.ValueObjects;
using FluentAssertions;

namespace Doctorly.Calendar.Tests.Domain
{
    public class EmailAddressTests
    {
        [Fact]
        public void Create_Should_Throw_When_EmailIsInvalid()
        {
            // Act
            var act = () => EmailAddress.Create("email");

            // Assert
            act.Should().Throw<DomainException>().WithMessage("*Email format is invalid*");
        }

        [Fact]
        public void Create_Should_Return_ValidEmail()
        {
            // Act
            var email = EmailAddress.Create("abc@docterly.de");

            // Assert
            email.Value.Should().Be("abc@docterly.de");
        }
    }
}
