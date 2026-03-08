namespace Doctorly.Calendar.Application.Common.Models
{
    public record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize);
}
