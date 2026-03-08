using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Domain.Entities;
using Doctorly.Calendar.Domain.Enums;
using Doctorly.Calendar.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Doctorly.Calendar.Infrastructure.Repositories
{
    public class CalendarEventRepository : ICalendarEventRepository
    {
        private readonly CalendarDbContext _dbContext;

        public CalendarEventRepository(CalendarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken)
        {
            await _dbContext.CalendarEvents.AddAsync(calendarEvent, cancellationToken);
        }

        public async Task<CalendarEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.CalendarEvents
                .Include(x => x.Attendees)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<CalendarEvent?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.CalendarEvents
                .Include(x => x.Attendees)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<CalendarEvent>> GetPagedEventsListAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _dbContext.CalendarEvents
                .Include(x => x.Attendees)
                .AsNoTracking()
                .OrderBy(x => x.StartTimeUtc)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<CalendarEvent>> SearchAsync(
            string? doctorName,
            string? patientName,
            DateTime? fromUtc,
            DateTime? toUtc,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.CalendarEvents
                .Include(x => x.Attendees)
                .AsNoTracking()
                .AsQueryable();

            //Added simple contain search for this assessment but in real world application
            //we would want to use more robust search capabilities (e.g. full text search or external search engine like Elasticsearch)

            if (!string.IsNullOrWhiteSpace(doctorName))
                query = query.Where(x => x.DoctorName.Contains(doctorName));

            if (!string.IsNullOrWhiteSpace(patientName))
                query = query.Where(x => x.PatientName.Contains(patientName));

            if (fromUtc.HasValue)
                query = query.Where(x => x.EndTimeUtc >= fromUtc.Value);

            if (toUtc.HasValue)
                query = query.Where(x => x.StartTimeUtc <= toUtc.Value);

            return await query.OrderBy(x => x.StartTimeUtc).ToListAsync(cancellationToken);
        }

        public async Task<bool> HasOverlappingEventAsync(
            string doctorName,
            DateTime startUtc,
            DateTime endUtc,
            Guid? excludeEventId,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.CalendarEvents.Where(x =>
                x.Status == CalendarEventStatus.Scheduled &&
                x.DoctorName == doctorName &&
                x.StartTimeUtc < endUtc &&
                x.EndTimeUtc > startUtc);

            if (excludeEventId.HasValue)
                query = query.Where(x => x.Id != excludeEventId.Value);

            return await query.AnyAsync(cancellationToken);
        }

        public void SetOriginalRowVersion(CalendarEvent calendarEvent, byte[] rowVersion)
        {
            _dbContext.Entry(calendarEvent).Property(x => x.RowVersion).OriginalValue = rowVersion;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
