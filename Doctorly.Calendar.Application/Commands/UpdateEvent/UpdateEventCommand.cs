using Doctorly.Calendar.Application.Common.Models;
using MediatR;

namespace Doctorly.Calendar.Application.Commands.UpdateEvent
{
    public record UpdateEventCommand(
       Guid CalendarEventId,
       string Title,
       string Description,
       string DoctorName,
       string PatientName,
       DateTime StartTimeUtc,
       DateTime EndTimeUtc,
       string RowVersion) : IRequest<EventDto>;
}
