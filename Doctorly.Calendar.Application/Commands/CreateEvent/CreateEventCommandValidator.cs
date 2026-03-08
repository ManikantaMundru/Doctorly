using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctorly.Calendar.Application.Commands.CreateEvent
{
    public class CreateEventCommandValidator: AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DoctorName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.PatientName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.StartTimeUtc).NotEmpty();
            RuleFor(x => x.EndTimeUtc).GreaterThan(x => x.StartTimeUtc);

            RuleForEach(x => x.Attendees).ChildRules(attendee =>
            {
                attendee.RuleFor(a => a.Email).NotEmpty().EmailAddress();
                attendee.RuleFor(a => a.Name).NotEmpty().MaximumLength(200);
            });
        }
    }
}
