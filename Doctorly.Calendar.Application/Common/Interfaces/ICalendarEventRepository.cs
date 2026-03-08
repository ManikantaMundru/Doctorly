using Doctorly.Calendar.Domain.Entities;

namespace Doctorly.Calendar.Application.Common.Interfaces
{
    public interface ICalendarEventRepository
    {
        Task AddAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken);
        Task<CalendarEvent> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<CalendarEvent> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<CalendarEvent>> GetPagedEventsListAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<IReadOnlyList<CalendarEvent>> SearchAsync(
            string doctorName,
            string patientName,
            DateTime? fromUtc,
            DateTime? toUtc,
            CancellationToken cancellationToken);
        Task<bool> HasOverlappingEventAsync(
            string doctorName,
            DateTime startUtc,
            DateTime endUtc,
            Guid? excludeEventId,
            CancellationToken cancellationToken);
        void SetOriginalRowVersion(CalendarEvent calendarEvent, byte[] rowVersion);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
