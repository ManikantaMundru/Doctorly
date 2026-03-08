using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Application.Common.Models;
using Doctorly.Calendar.Domain.Entities;
using Doctorly.Calendar.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctorly.Calendar.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IIcalBuilder _icalBuilder;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IIcalBuilder icalBuilder, ILogger<NotificationService> logger)
        {
            _icalBuilder = icalBuilder;
            _logger = logger;
        }

        public Task NotifyEventCreatedAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken)
        {
           var message = BuildMessage(
           subject: $"Event created: {calendarEvent.Title}",
           body: $"""
                    A calendar event has been created.

                    Title: {calendarEvent.Title}
                    Doctor: {calendarEvent.DoctorName}
                    Patient: {calendarEvent.PatientName}
                    Start: {calendarEvent.StartTimeUtc:u}
                    End: {calendarEvent.EndTimeUtc:u}
                    """,
           recipients: calendarEvent.Attendees.Select(x => x.Email).ToList(),
           icalContent: _icalBuilder.BuildEventInvite(calendarEvent));
           LogMessage("EventCreated", calendarEvent.Id, message);

            return Task.CompletedTask;
        }

        public Task NotifyEventUpdatedAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken)
        {
             var message = BuildMessage(
             subject: $"Event updated: {calendarEvent.Title}",
             body: $"""
                        A calendar event has been updated.

                        Title: {calendarEvent.Title}
                        Doctor: {calendarEvent.DoctorName}
                        Patient: {calendarEvent.PatientName}
                        Start: {calendarEvent.StartTimeUtc:u}
                        End: {calendarEvent.EndTimeUtc:u}
                        """,
             recipients: calendarEvent.Attendees.Select(x => x.Email).ToList(),
             icalContent: _icalBuilder.BuildEventInvite(calendarEvent));

            LogMessage("EventUpdated", calendarEvent.Id, message);

            return Task.CompletedTask;
        }

        public Task NotifyEventCancelledAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken)
        {
             var message = BuildMessage(
             subject: $"Event cancelled: {calendarEvent.Title}",
             body: $"""
                        A calendar event has been cancelled.

                        Title: {calendarEvent.Title}
                        Doctor: {calendarEvent.DoctorName}
                        Patient: {calendarEvent.PatientName}
                        """,
             recipients: calendarEvent.Attendees.Select(x => x.Email).ToList(),
             icalContent: _icalBuilder.BuildEventInvite(calendarEvent));

            LogMessage("EventCancelled", calendarEvent.Id, message);
            return Task.CompletedTask;
        }

        public Task NotifyInvitationRespondedAsync(CalendarEvent calendarEvent, Guid attendeeId, AttendeeResponseStatus responseStatus, CancellationToken cancellationToken)
        {
            var attendee = calendarEvent.Attendees.FirstOrDefault(x => x.Id == attendeeId);

            var recipients = new List<string>();
            if (attendee is not null)
            {
                recipients.Add(attendee.Email);
            }

            var message = BuildMessage(
                subject: $"Invitation response: {calendarEvent.Title}",
                body: $"""
                        An attendee responded to the invitation.

                        Event: {calendarEvent.Title}
                        AttendeeId: {attendeeId}
                        Response: {responseStatus}
                        """,
                recipients: recipients,
                icalContent: null);

            LogMessage("InvitationResponded", calendarEvent.Id, message);
            return Task.CompletedTask;
        }

        private static NotificationMessage BuildMessage(
           string subject,
           string body,
           IReadOnlyList<string> recipients,
           string? icalContent)
        {
            return new NotificationMessage
            {
                Subject = subject,
                Body = body,
                Recipients = recipients,
                IcalContent = icalContent
            };
        }

        private void LogMessage(string eventType, Guid eventId, NotificationMessage message)
        {
            _logger.LogInformation(
                """
                    Notification dispatched
                    Type: {EventType}
                    EventId: {EventId}
                    Subject: {Subject}
                    Recipients: {Recipients}
                    Body: {Body}
                    IcalAttached: {HasIcal}
                    """,
                eventType,
                eventId,
                message.Subject,
                string.Join(", ", message.Recipients),
                message.Body,
                !string.IsNullOrWhiteSpace(message.IcalContent));
        }
    }
}
