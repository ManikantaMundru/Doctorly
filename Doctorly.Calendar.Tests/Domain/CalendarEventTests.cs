using Doctorly.Calendar.Domain.Common;
using Doctorly.Calendar.Domain.Entities;
using Doctorly.Calendar.Domain.Enums;
using Doctorly.Calendar.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctorly.Calendar.Tests.Domain
{
    public class CalendarEventTests
    {
        [Fact]
        public void Update_ShouldThrow_When_EventIsCancelled()
        {
            // Arrange
            var attendee = new Attendee("patient@test.com", "Patient");

            var calendarEvent = new CalendarEvent(
                "Eye Test",
                "Consultation",
                "Dr Mani",
                "Patient",
                new EventTimeRange(
                    new DateTime(2026, 3, 10, 9, 0, 0, DateTimeKind.Utc),
                    new DateTime(2026, 3, 10, 10, 0, 0, DateTimeKind.Utc)),
                DateTime.UtcNow,
                new List<Attendee> { attendee });

            calendarEvent.Cancel();

            // Act
            var act = () => calendarEvent.Update(
                "Updated Title",
                "Updated Description",
                "Dr Mani",
                "David",
                new EventTimeRange(
                    new DateTime(2026, 3, 10, 11, 0, 0, DateTimeKind.Utc),
                    new DateTime(2026, 3, 10, 12, 0, 0, DateTimeKind.Utc)));

            // Assert
            act.Should().Throw<DomainException>().WithMessage("*cannot be modified*");
        }

        [Fact]
        public void RespondToInvitation_ShouldThrow_When_AttendeeDoesNotExist()
        {
            // Arrange
            var attendee = new Attendee("patient@test.com", "Patient");

            var calendarEvent = new CalendarEvent(
                "Eye Test",
                "Consultation",
                "Dr Mani",
                "Patient",
                new EventTimeRange(
                    new DateTime(2026, 3, 10, 9, 0, 0, DateTimeKind.Utc),
                    new DateTime(2026, 3, 10, 10, 0, 0, DateTimeKind.Utc)),
                DateTime.UtcNow,
                new List<Attendee> { attendee });

            // Act
            var act = () => calendarEvent.RespondToInvitation(Guid.NewGuid(), AttendeeResponseStatus.Accepted);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("*Attendee not found*");
        }

        [Fact]
        public void RespondToInvitation_Should_Update_The_Correct_Attendee_Response()
        {
            // Arrange
            var attendee1 = new Attendee("patient1@test.com", "Patient1");
            var attendee2 = new Attendee("patient2@test.com", "Patient2");

            var calendarEvent = new CalendarEvent(
                "Eye Test",
                "Consultation",
                "Dr Mani",
                "Patients",
                new EventTimeRange(
                    new DateTime(2026, 3, 10, 9, 0, 0, DateTimeKind.Utc),
                    new DateTime(2026, 3, 10, 10, 0, 0, DateTimeKind.Utc)),
                DateTime.UtcNow,
                new List<Attendee> { attendee1, attendee2 });

            // Act
            calendarEvent.RespondToInvitation(attendee2.Id, AttendeeResponseStatus.Accepted);

            // Assert
            attendee1.ResponseStatus.Should().Be(AttendeeResponseStatus.Pending);
            attendee2.ResponseStatus.Should().Be(AttendeeResponseStatus.Accepted);
            attendee2.RespondedAtUtc.Should().NotBeNull();
        }
    }
}
