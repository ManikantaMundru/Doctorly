using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Domain.Entities;

namespace Doctorly.Calendar.Infrastructure.Services
{
    public class IcalBuilderService : IIcalBuilder
    {
        public string BuildEventInvite(CalendarEvent calendarEvent)
        {
            var description = Escape(calendarEvent.Description ?? string.Empty);
            var summary = Escape(calendarEvent.Title);
            var uid = calendarEvent.Id.ToString();

            return $"""
                    BEGIN:VCALENDAR
                    VERSION:2.0
                    PRODID:-//Doctorly Calendar//EN
                    CALSCALE:GREGORIAN
                    METHOD:REQUEST
                    BEGIN:VEVENT
                    UID:{uid}
                    DTSTAMP:{DateTime.UtcNow:yyyyMMdd'T'HHmmss'Z'}
                    DTSTART:{calendarEvent.StartTimeUtc:yyyyMMdd'T'HHmmss'Z'}
                    DTEND:{calendarEvent.EndTimeUtc:yyyyMMdd'T'HHmmss'Z'}
                    SUMMARY:{summary}
                    DESCRIPTION:{description}
                    STATUS:{GetStatus(calendarEvent)}
                    END:VEVENT
                    END:VCALENDAR
                    """;
        }

        private static string Escape(string value)
        {
            return value
                .Replace("\\", "\\\\")
                .Replace(";", "\\;")
                .Replace(",", "\\,")
                .Replace("\r\n", "\\n")
                .Replace("\n", "\\n");
        }

        private static string GetStatus(CalendarEvent calendarEvent)
        {
            return calendarEvent.Status.ToString().ToUpperInvariant() switch
            {
                "CANCELLED" => "CANCELLED",
                _ => "SCHEDULED"
            };
        }
    }
}
