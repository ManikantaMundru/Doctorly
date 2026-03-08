using Doctorly.Calendar.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctorly.Calendar.Application.Queries.SearchEvents
{
    public record SearchEventsQuery(
       string DoctorName,
       string PatientName,
       DateTime? FromUtc,
       DateTime? ToUtc) : IRequest<IReadOnlyList<EventDto>>;
}
