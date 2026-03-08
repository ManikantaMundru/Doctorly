using Doctorly.Calendar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctorly.Calendar.Infrastructure.Persistence.Configurations
{
    public class CalendarEventConfiguration : IEntityTypeConfiguration<CalendarEvent>
    {
        public void Configure(EntityTypeBuilder<CalendarEvent> builder)
        {
            builder.ToTable("CalendarEvents");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(2000);
            builder.Property(x => x.DoctorName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.PatientName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.StartTimeUtc).IsRequired();
            builder.Property(x => x.EndTimeUtc).IsRequired();
            builder.Property(x => x.CreatedAtUtc).IsRequired();

            builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            builder.HasMany(x => x.Attendees)
                .WithOne()
                .HasForeignKey(x => x.CalendarEventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.DoctorName, x.StartTimeUtc, x.EndTimeUtc });
        }
    }
}
