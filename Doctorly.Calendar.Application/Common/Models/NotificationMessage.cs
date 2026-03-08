namespace Doctorly.Calendar.Application.Common.Models
{
    public class NotificationMessage
    {
        public string Subject { get; init; } = string.Empty;
        public string Body { get; init; } = string.Empty;
        public IReadOnlyList<string> Recipients { get; init; } = Array.Empty<string>();
        public string IcalContent { get; init; }
    }
}
