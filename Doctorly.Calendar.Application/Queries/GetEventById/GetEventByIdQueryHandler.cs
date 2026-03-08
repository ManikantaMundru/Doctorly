using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Application.Common.Mappings;
using Doctorly.Calendar.Application.Common.Models;
using MediatR;

namespace Doctorly.Calendar.Application.Queries.GetEventById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto>
    {
        private readonly ICalendarEventRepository _repository;

        public GetEventByIdQueryHandler(ICalendarEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var calendarEvent = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (calendarEvent is null)
                throw new KeyNotFoundException("Event not found.");

            return calendarEvent.ToDto();
        }
    }

}
