namespace Doctorly.Calendar.Application.Common.Models
{
    public sealed record EventDto(
      Guid Id,
      string Title,
      string Description,
      string DoctorName,
      string PatientName,
      DateTime StartTimeUtc,
      DateTime EndTimeUtc,
      string Status,
      DateTime CreatedAtUtc,
      DateTime? UpdatedAtUtc,
      string RowVersion,
      IReadOnlyList<AttendeeDto> Attendees);
}
