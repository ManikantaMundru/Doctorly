using Doctorly.Calendar.Application.Commands.CancelEvent;
using Doctorly.Calendar.Application.Commands.CreateEvent;
using Doctorly.Calendar.Application.Commands.RespondToInvite;
using Doctorly.Calendar.Application.Commands.UpdateEvent;
using Doctorly.Calendar.Application.Queries.GetEventById;
using Doctorly.Calendar.Application.Queries.ListEvents;
using Doctorly.Calendar.Application.Queries.SearchEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctorly.Calendar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEventCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Cancel([FromBody] CancelEventCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEventByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new ListEventsQuery(page, pageSize), cancellationToken);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? doctorName,
            [FromQuery] string? patientName,
            [FromQuery] DateTime? fromUtc,
            [FromQuery] DateTime? toUtc,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new SearchEventsQuery(doctorName, patientName, fromUtc, toUtc), cancellationToken);
            return Ok(result);
        }

        [HttpPost("respond")]
        public async Task<IActionResult> Respond([FromBody] RespondToInvitationCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new RespondToInvitationCommand(request.EventId, request.AttendeeId, request.ResponseStatus, request.RowVersion),
                cancellationToken);

            return Ok(result);
        }
    }
}
