using Doctorly.Calendar.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctorly.Calendar.Application.Commands.CreateEvent
{
    public record CreateEventAttendeeRequest(string Email, string Name);

    public record CreateEventCommand(
        string Title,
        string Description,
        string DoctorName,
        string PatientName,
        DateTime StartTimeUtc,
        DateTime EndTimeUtc,
        IReadOnlyList<CreateEventAttendeeRequest> Attendees) : IRequest<EventDto>;
}
