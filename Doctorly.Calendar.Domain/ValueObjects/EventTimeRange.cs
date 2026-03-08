using Doctorly.Calendar.Domain.Common;

namespace Doctorly.Calendar.Domain.ValueObjects
{
    public class EventTimeRange
    {
        public DateTime StartTimeUtc { get; }
        public DateTime EndTimeUtc { get; }

        public EventTimeRange(DateTime startTimeUtc, DateTime endTimeUtc)
        {
            if (startTimeUtc.Kind != DateTimeKind.Utc)
                throw new DomainException("StartUtc must be UTC.");

            if (endTimeUtc.Kind != DateTimeKind.Utc)
                throw new DomainException("EndUtc must be UTC.");

            if (endTimeUtc <= startTimeUtc)
                throw new DomainException("End time must be after start time.");

            StartTimeUtc = startTimeUtc;
            EndTimeUtc = endTimeUtc;
        }

        public bool Overlaps(EventTimeRange timeRange)
            => StartTimeUtc < timeRange.EndTimeUtc && EndTimeUtc > timeRange.StartTimeUtc;
    }
}
