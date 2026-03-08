using Doctorly.Calendar.Domain.Entities;
using Doctorly.Calendar.Domain.Enums;
using FluentAssertions;

namespace Doctorly.Calendar.Tests.Domain
{
    public class AttendeeTests
    {
        [Fact]
        public void ResetResponse_Should_Set_StatusBackToPending()
        {
            // Arrange
            var attendee = new Attendee("patient@test.com", "Patient1");
            attendee.Respond(AttendeeResponseStatus.Accepted, DateTime.UtcNow);

            // Act
            attendee.ResetResponse();

            // Assert
            attendee.ResponseStatus.Should().Be(AttendeeResponseStatus.Pending);
            attendee.RespondedAtUtc.Should().BeNull();
        }

        [Fact]
        public void Respond_Should_Update_ResponseStatusAndTime()
        {
            // Arrange
            var attendee = new Attendee("patient@test.com", "Patient1");
            var respondedAt = new DateTime(2026, 3, 10, 8, 0, 0, DateTimeKind.Utc);

            // Act
            attendee.Respond(AttendeeResponseStatus.Accepted, respondedAt);

            // Assert
            attendee.ResponseStatus.Should().Be(AttendeeResponseStatus.Accepted);
            attendee.RespondedAtUtc.Should().Be(respondedAt);
        }
    }
}
