using Doctorly.Calendar.Application.Common.Models;
using MediatR;

namespace Doctorly.Calendar.Application.Commands.CancelEvent
{
    public record CancelEventCommand(Guid CalendarEventId, string RowVersion) : IRequest<EventDto>;
}
