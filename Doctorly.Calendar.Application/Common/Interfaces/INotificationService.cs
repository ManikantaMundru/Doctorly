using Doctorly.Calendar.Domain.Entities;
using Doctorly.Calendar.Domain.Enums;

namespace Doctorly.Calendar.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task NotifyEventCreatedAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken);
        Task NotifyEventUpdatedAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken);
        Task NotifyEventCancelledAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken);
        Task NotifyInvitationRespondedAsync(CalendarEvent calendarEvent, Guid attendeeId, AttendeeResponseStatus responseStatus, CancellationToken cancellationToken);
    }
}
