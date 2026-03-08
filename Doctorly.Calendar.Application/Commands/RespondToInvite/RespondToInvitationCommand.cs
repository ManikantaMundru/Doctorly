using Doctorly.Calendar.Application.Common.Models;
using Doctorly.Calendar.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctorly.Calendar.Application.Commands.RespondToInvite
{
    public record RespondToInvitationCommand(
      Guid EventId,
      Guid AttendeeId,
      AttendeeResponseStatus ResponseStatus,
      string RowVersion) : IRequest<EventDto>;
}
