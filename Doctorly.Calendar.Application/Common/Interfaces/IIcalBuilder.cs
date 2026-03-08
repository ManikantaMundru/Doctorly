using Doctorly.Calendar.Domain.Entities;

namespace Doctorly.Calendar.Application.Common.Interfaces
{
    public interface IIcalBuilder
    {
        string BuildEventInvite(CalendarEvent calendarEvent);
    }
}
