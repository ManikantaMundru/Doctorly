namespace Doctorly.Calendar.Application.Common.Models
{
    public record AttendeeDto(
       Guid Id,
       string Email,
       string Name,
       string ResponseStatus,
       DateTime? RespondedAtUtc);
}
