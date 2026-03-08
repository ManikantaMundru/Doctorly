using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Application.Common.Mappings;
using Doctorly.Calendar.Application.Common.Models;
using Doctorly.Calendar.Domain.Common;
using Doctorly.Calendar.Domain.ValueObjects;
using MediatR;

namespace Doctorly.Calendar.Application.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler: IRequestHandler<UpdateEventCommand, EventDto>
    {
        private readonly ICalendarEventRepository _repository;
        private readonly INotificationService _notificationService;

        public UpdateEventCommandHandler(ICalendarEventRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task<EventDto> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var calendarEvent = await _repository.GetByIdForUpdateAsync(request.CalendarEventId, cancellationToken);

            if (calendarEvent is null)
                throw new KeyNotFoundException("Event not found.");

            var originalRowVersion = Convert.FromBase64String(request.RowVersion);
            _repository.SetOriginalRowVersion(calendarEvent, originalRowVersion);

            var hasOverlap = await _repository.HasOverlappingEventAsync(
                request.DoctorName,
                request.StartTimeUtc,
                request.EndTimeUtc,
                request.CalendarEventId,
                cancellationToken);

            if (hasOverlap)
                throw new DomainException("The doctor already has another event during the requested time range.");

            calendarEvent.Update(
                request.Title,
                request.Description,
                request.DoctorName,
                request.PatientName,
                new EventTimeRange(request.StartTimeUtc, request.EndTimeUtc));
         
            await _repository.SaveChangesAsync(cancellationToken);

            await _notificationService.NotifyEventUpdatedAsync(
                calendarEvent,
                cancellationToken);

            return calendarEvent.ToDto();
        }
    }
}
