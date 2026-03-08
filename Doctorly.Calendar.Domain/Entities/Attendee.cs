using Doctorly.Calendar.Domain.Common;
using Doctorly.Calendar.Domain.Enums;
using Doctorly.Calendar.Domain.ValueObjects;

namespace Doctorly.Calendar.Domain.Entities
{
    public class Attendee : Entity
    {
        public Guid CalendarEventId { get; private set; }
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public DateTime? RespondedAtUtc { get; private set; }
        public AttendeeResponseStatus ResponseStatus { get; private set; }

        private Attendee()
        {
        }

        public Attendee(string email, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Attendee name is required.");

            var trimmedName = name.Trim();

            if (trimmedName.Length > 100)
                throw new DomainException("Attendee name must not exceed 100 characters.");

            var emailAddress = EmailAddress.Create(email);

            Id = Guid.NewGuid();
            Name = trimmedName;
            Email = emailAddress.Value;
            ResponseStatus = AttendeeResponseStatus.Pending;
        }

        public void Respond(AttendeeResponseStatus responseStatus, DateTime respondedAtUtc)
        {
            ResponseStatus = responseStatus;
            RespondedAtUtc = respondedAtUtc;
        }

        public void ResetResponse()
        {
            // reset only if previously responded
            if (ResponseStatus != AttendeeResponseStatus.Pending)
            {
                ResponseStatus = AttendeeResponseStatus.Pending;
                RespondedAtUtc = null;
            }
        }
    }
}
