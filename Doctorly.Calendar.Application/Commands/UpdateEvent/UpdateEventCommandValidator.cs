using FluentValidation;

namespace Doctorly.Calendar.Application.Commands.UpdateEvent
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            RuleFor(x => x.CalendarEventId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DoctorName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.PatientName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.StartTimeUtc).NotEmpty();
            RuleFor(x => x.EndTimeUtc).GreaterThan(x => x.StartTimeUtc);
            RuleFor(x => x.RowVersion).NotEmpty();
        }
    }
}