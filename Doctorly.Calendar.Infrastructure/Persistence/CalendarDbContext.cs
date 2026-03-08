using Doctorly.Calendar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doctorly.Calendar.Infrastructure.Persistence
{
    public class CalendarDbContext : DbContext
    {
        public CalendarDbContext(DbContextOptions<CalendarDbContext> options) : base(options)
        {
        }

        public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();
        public DbSet<Attendee> Attendees => Set<Attendee>();

        public void SetOriginalRowVersion(CalendarEvent calendarEvent, byte[] rowVersion)
        {
            Entry(calendarEvent).Property(x => x.RowVersion).OriginalValue = rowVersion;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CalendarDbContext).Assembly);
        }
    }
}
