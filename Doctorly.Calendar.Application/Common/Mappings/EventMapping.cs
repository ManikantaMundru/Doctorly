using Doctorly.Calendar.Application.Common.Models;
using Doctorly.Calendar.Domain.Entities;

namespace Doctorly.Calendar.Application.Common.Mappings
{
    public static class EventMappings
    {
        public static EventDto ToDto(this CalendarEvent entity)
            => new(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.DoctorName,
                entity.PatientName,
                entity.StartTimeUtc,
                entity.EndTimeUtc,
                entity.Status.ToString(),
                entity.CreatedAtUtc,
                entity.UpdatedAtUtc,
                Convert.ToBase64String(entity.RowVersion ?? Array.Empty<byte>()),
                entity.Attendees
                    .Select(a => new AttendeeDto(
                        a.Id,
                        a.Email,
                        a.Name,
                        a.ResponseStatus.ToString(),
                        a.RespondedAtUtc))
                    .ToList());
    }
}
