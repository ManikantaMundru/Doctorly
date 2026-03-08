using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Application.Common.Mappings;
using Doctorly.Calendar.Application.Common.Models;
using MediatR;

namespace Doctorly.Calendar.Application.Commands.RespondToInvite
{
    public class RespondToInvitationHandler : IRequestHandler<RespondToInvitationCommand, EventDto>
    {
        private readonly ICalendarEventRepository _repository;
        private readonly INotificationService _notificationService;

        public RespondToInvitationHandler(
            ICalendarEventRepository repository,
            INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task<EventDto> Handle(RespondToInvitationCommand request, CancellationToken cancellationToken)
        {
            var calendarEvent = await _repository.GetByIdForUpdateAsync(request.EventId, cancellationToken);

            if (calendarEvent is null)
                throw new KeyNotFoundException("Event not found.");

            var originalRowVersion = Convert.FromBase64String(request.RowVersion);
            _repository.SetOriginalRowVersion(calendarEvent, originalRowVersion);

            calendarEvent.RespondToInvitation(request.AttendeeId, request.ResponseStatus);

            await _repository.SaveChangesAsync(cancellationToken);

            //await _notificationService.SendInvitationResponseAsync(
            //    calendarEvent.Id,
            //    calendarEvent.Attendees.FirstOrDefault(x => x.Id == request.AttendeeId)?.Email,
            //    request.ResponseStatus,
            //    cancellationToken);

            return calendarEvent.ToDto();
        }
    }
}
