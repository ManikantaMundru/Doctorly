using Doctorly.Calendar.Application.Common.Models;
using MediatR;

namespace Doctorly.Calendar.Application.Queries.GetEventById
{
    public record GetEventByIdQuery(Guid Id) : IRequest<EventDto>;
}
