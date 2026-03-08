using Doctorly.Calendar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctorly.Calendar.Infrastructure.Persistence.Configurations
{
    public class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
    {
        public void Configure(EntityTypeBuilder<Attendee> builder)
        {
            builder.ToTable("Attendees");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email).IsRequired().HasMaxLength(320);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ResponseStatus).IsRequired();

            builder.HasIndex(x => new { x.CalendarEventId, x.Email }).IsUnique();
        }
    }
}
