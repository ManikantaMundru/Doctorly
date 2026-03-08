using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Application.Common.Mappings;
using Doctorly.Calendar.Application.Common.Models;
using Doctorly.Calendar.Domain.Common;
using Doctorly.Calendar.Domain.Entities;
using Doctorly.Calendar.Domain.ValueObjects;
using MediatR;

namespace Doctorly.Calendar.Application.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
    {
        private readonly ICalendarEventRepository _repository;
        private readonly INotificationService _notificationService;

        public CreateEventCommandHandler(
            ICalendarEventRepository repository,
            INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var hasOverlap = await _repository.HasOverlappingEventAsync(
                request.DoctorName,
                request.StartTimeUtc,
                request.EndTimeUtc,
                null,
                cancellationToken);

            if (hasOverlap)
                throw new DomainException("The doctor already has another event during the requested time range.");

            var attendees = request.Attendees
                .Select(a => new Attendee(a.Email, a.Name))
                .ToList();

            var calendarEvent = new CalendarEvent(
                request.Title,
                request.Description,
                request.DoctorName,
                request.PatientName,
                new EventTimeRange(request.StartTimeUtc, request.EndTimeUtc),
                DateTime.UtcNow,
                attendees);

            await _repository.AddAsync(calendarEvent, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            await _notificationService.NotifyEventCreatedAsync(
                calendarEvent,
                cancellationToken);

            return calendarEvent.ToDto();
        }
    }
}
