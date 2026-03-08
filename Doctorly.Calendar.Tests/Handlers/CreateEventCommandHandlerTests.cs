using Doctorly.Calendar.Application.Commands.CreateEvent;
using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Domain.Common;
using Doctorly.Calendar.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Doctorly.Calendar.Tests.Handlers
{
    public class CreateEventCommandHandlerTests
    {
        private readonly Mock<ICalendarEventRepository> mockCalendarEventRepository = new();
        private readonly Mock<INotificationService> mockNotificationService = new();
        private readonly CreateEventCommandHandler handler;

        private readonly CreateEventCommand validCommand = new(
            Title: "Eye Test",
            Description: "Eye consultation",
            DoctorName: "Dr Mani",
            PatientName: "Patient",
            StartTimeUtc: new(2026, 3, 10, 9, 0, 0, DateTimeKind.Utc),
            EndTimeUtc: new(2026, 3, 10, 10, 0, 0, DateTimeKind.Utc),
            Attendees: [new("patient@test.com", "Patient")]
        );

        public CreateEventCommandHandlerTests()
        {
            handler = new CreateEventCommandHandler(mockCalendarEventRepository.Object, mockNotificationService.Object);
        }
                

        [Fact]
        public async Task Should_create_event_and_notify_when_no_overlap()
        {
            mockCalendarEventRepository.Setup(r => r.HasOverlappingEventAsync(
                "Dr Mani", 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>(), 
                null, 
                It.IsAny<CancellationToken>()))
             .ReturnsAsync(false);

            var result = await handler.Handle(validCommand, CancellationToken.None);

            result.Should().NotBeNull();
            mockCalendarEventRepository.Verify(r => r.AddAsync(It.IsAny<CalendarEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            mockCalendarEventRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockNotificationService.Verify(n => n.NotifyEventCreatedAsync(It.IsAny<CalendarEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_throw_when_doctor_has_overlapping_event()
        {
            mockCalendarEventRepository.Setup(r => r.HasOverlappingEventAsync(
                "Dr Mani", 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>(), 
                null, 
                It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

            var act = async () => await handler.Handle(validCommand, CancellationToken.None);

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*already has another event*");

            mockCalendarEventRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            mockNotificationService.Verify(n => n.NotifyEventCreatedAsync(It.IsAny<CalendarEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}