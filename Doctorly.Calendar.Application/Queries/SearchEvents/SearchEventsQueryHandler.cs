using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Application.Common.Mappings;
using Doctorly.Calendar.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctorly.Calendar.Application.Queries.SearchEvents
{
    public class SearchEventsQueryHandler : IRequestHandler<SearchEventsQuery, IReadOnlyList<EventDto>>
    {
        private readonly ICalendarEventRepository _repository;

        public SearchEventsQueryHandler(ICalendarEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<EventDto>> Handle(SearchEventsQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.SearchAsync(
                request.DoctorName,
                request.PatientName,
                request.FromUtc,
                request.ToUtc,
                cancellationToken);

            return items.Select(x => x.ToDto()).ToList();
        }
    }
}
