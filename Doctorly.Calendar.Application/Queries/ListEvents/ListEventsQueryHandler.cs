using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Application.Common.Mappings;
using Doctorly.Calendar.Application.Common.Models;
using MediatR;

namespace Doctorly.Calendar.Application.Queries.ListEvents
{
    public class ListEventsQueryHandler : IRequestHandler<ListEventsQuery, PagedResult<EventDto>>
    {
        private readonly ICalendarEventRepository _repository;

        public ListEventsQueryHandler(ICalendarEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<EventDto>> Handle(ListEventsQuery request, CancellationToken cancellationToken)
        {
            var results = await _repository.GetPagedEventsListAsync(request.Page, request.PageSize, cancellationToken);

            return new PagedResult<EventDto>(results.Select(x => x.ToDto()).ToList(), request.Page, request.PageSize);
        }
    }
}