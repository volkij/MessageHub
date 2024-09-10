using MessageHub.Domain.Entities;
using FluentValidation;

namespace MessageHub.Domain.Validators
{
    public class MessageServiceValidator : AbstractValidator<Message>
    {
        public MessageServiceValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.");

            RuleFor(x => x.Subject)
                .NotEmpty()
                .When(x => x.Type.ToUpper() == "EMAIL" || x.Type.ToUpper() == "PUSH")
                .WithMessage("Subject is required for EMAIL and PUSH.");

            RuleFor(x => x.ContactValue)
                .NotEmpty()
                .When(x => x.Type.ToUpper() == "EMAIL" || x.Type.ToUpper() == "SMS")
                .WithMessage("ContactValue is required for EMAIL and SMS.");

            RuleFor(x => x.ExternalClientID)
                .NotEmpty()
                .When(x => x.Type.ToUpper() == "PUSH")
                .WithMessage("ExternalClientID is required for PUSH.");
        }
    }
}