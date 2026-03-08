using Doctorly.Calendar.Domain.Common;
using Doctorly.Calendar.Domain.Enums;
using Doctorly.Calendar.Domain.ValueObjects;

namespace Doctorly.Calendar.Domain.Entities
{
    public class CalendarEvent : AggregateRoot
    {
        private readonly List<Attendee> _attendees = [];

        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public string DoctorName { get; private set; } = null!;
        public string PatientName { get; private set; } = null!;
        public DateTime StartTimeUtc { get; private set; }
        public DateTime EndTimeUtc { get; private set; }
        public CalendarEventStatus Status { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime UpdatedAtUtc { get; private set; }
        public byte[] RowVersion { get; private set; } = [];
        public IReadOnlyCollection<Attendee> Attendees => _attendees.AsReadOnly();

        private CalendarEvent()
        {
        }

        public CalendarEvent(
            string title,
            string description,
            string doctorName,
            string patientName,
            EventTimeRange timeRange,
            DateTime createdAtUtc,
            IEnumerable<Attendee> attendees = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title is required.");

            if (string.IsNullOrWhiteSpace(doctorName))
                throw new DomainException("Doctor name is required.");

            if (string.IsNullOrWhiteSpace(patientName))
                throw new DomainException("Patient name is required.");

            Id = Guid.NewGuid();
            Title = title.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            DoctorName = doctorName.Trim();
            PatientName = patientName.Trim();
            StartTimeUtc = timeRange.StartTimeUtc;
            EndTimeUtc = timeRange.EndTimeUtc;
            CreatedAtUtc = createdAtUtc;
            Status = CalendarEventStatus.Scheduled;

            SetAttendees(attendees);
        }

        public void Update(
           string title,
           string description,
           string doctorName,
           string patientName,
           EventTimeRange timeRange)
        {
            EnsureNotCancelled();

            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title is required.");

            if (string.IsNullOrWhiteSpace(doctorName))
                throw new DomainException("Doctor name is required.");

            if (string.IsNullOrWhiteSpace(patientName))
                throw new DomainException("Patient name is required.");

            Title = title.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            DoctorName = doctorName.Trim();
            PatientName = patientName.Trim();
            StartTimeUtc = timeRange.StartTimeUtc;
            EndTimeUtc = timeRange.EndTimeUtc;
            UpdatedAtUtc = DateTime.UtcNow;

            //Reset the response status of attendees for the time slots update
            var timeChanged = StartTimeUtc != timeRange.StartTimeUtc || EndTimeUtc != timeRange.EndTimeUtc;

            if (timeChanged)
            {
                foreach (var attendee in _attendees)
                    attendee.ResetResponse();
            }
        }

        public void Cancel()
        {
            EnsureNotCancelled();
            Status = CalendarEventStatus.Cancelled;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void RespondToInvitation(Guid attendeeId, AttendeeResponseStatus status)
        {
            EnsureNotCancelled();

            var attendee = _attendees.FirstOrDefault(x => x.Id == attendeeId);

            if (attendee is null)
                throw new DomainException("Attendee not found for this event.");

            attendee.Respond(status, DateTime.UtcNow);
            UpdatedAtUtc = DateTime.UtcNow;
        }

        private void SetAttendees(IEnumerable<Attendee> attendees)
        {
            if (attendees is null)
                throw new DomainException("At least one attendee is required.");

            var attendeeList = attendees.ToList();

            if (attendeeList.Count == 0)
                throw new DomainException("At least one attendee is required.");

            var duplicates = attendeeList
                .GroupBy(x => x.Email)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            if (duplicates.Count > 0)
                throw new DomainException("Duplicate attendee emails are not allowed.");

            _attendees.AddRange(attendeeList);
        }

        private void EnsureNotCancelled()
        {
            if (Status == CalendarEventStatus.Cancelled)
                throw new DomainException("Cancelled events cannot be modified.");
        }
    }
}
