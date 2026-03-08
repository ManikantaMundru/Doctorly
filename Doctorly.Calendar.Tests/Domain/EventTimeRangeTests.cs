using Doctorly.Calendar.Domain.Common;
using Doctorly.Calendar.Domain.ValueObjects;
using FluentAssertions;

namespace Doctorly.Calendar.Tests.Domain
{
    public class EventTimeRangeTests
    {
        [Fact]
        public void EventTimeRange_Should_Throw_When_EndDateIsNotAfterStartDate()
        {
            // Arrange
            var start = new DateTime(2026, 3, 10, 9, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(2026, 3, 10, 9, 0, 0, DateTimeKind.Utc);

            // Act
            var act = () => new EventTimeRange(start, end);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("*after start*");
        }

        [Fact]
        public void EventTimeRange_Should_Create_WhenRangeIsValid()
        {
            // Arrange
            var start = new DateTime(2026, 3, 10, 9, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(2026, 3, 10, 10, 0, 0, DateTimeKind.Utc);

            // Act
            var range = new EventTimeRange(start, end);

            // Assert
            range.StartTimeUtc.Should().Be(start);
            range.EndTimeUtc.Should().Be(end);
        }
    }
}
