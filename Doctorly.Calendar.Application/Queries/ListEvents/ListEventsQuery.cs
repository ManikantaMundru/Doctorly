using Doctorly.Calendar.Application.Common.Models;
using MediatR;

namespace Doctorly.Calendar.Application.Queries.ListEvents
{
    public record ListEventsQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResult<EventDto>>;
}
