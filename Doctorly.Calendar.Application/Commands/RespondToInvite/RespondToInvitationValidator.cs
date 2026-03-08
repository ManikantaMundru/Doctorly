using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctorly.Calendar.Application.Commands.RespondToInvite
{
    public class RespondToInvitationValidator : AbstractValidator<RespondToInvitationCommand>
    {
        public RespondToInvitationValidator()
        {
            RuleFor(x => x.EventId).NotEmpty();
            RuleFor(x => x.AttendeeId).NotEmpty();
            RuleFor(x => x.RowVersion).NotEmpty();
        }
    }
}
