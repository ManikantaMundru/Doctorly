using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Application.Common.Mappings;
using Doctorly.Calendar.Application.Common.Models;
using MediatR;

namespace Doctorly.Calendar.Application.Commands.CancelEvent
{
    public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand, EventDto>
    {
        private readonly ICalendarEventRepository _repository;
        private readonly INotificationService _notificationService;

        public CancelEventCommandHandler(
            ICalendarEventRepository repository,
            INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task<EventDto> Handle(CancelEventCommand request, CancellationToken cancellationToken)
        {
            var calendarEvent = await _repository.GetByIdForUpdateAsync(request.CalendarEventId, cancellationToken);

            if (calendarEvent is null)
                throw new KeyNotFoundException("Event not found.");

            var originalRowVersion = Convert.FromBase64String(request.RowVersion);
            _repository.SetOriginalRowVersion(calendarEvent, originalRowVersion);

            calendarEvent.Cancel();

            await _repository.SaveChangesAsync(cancellationToken);

            await _notificationService.NotifyEventCancelledAsync(
                calendarEvent,
                cancellationToken);

            return calendarEvent.ToDto();
        }
    }
}
