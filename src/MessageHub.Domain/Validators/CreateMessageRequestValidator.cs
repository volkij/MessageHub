using MessageHub.Domain.DTO;
using FluentValidation;

namespace MessageHub.Domain.Validators
{
    public class CreateMessageRequestValidator : AbstractValidator<CreateMessageRequest>
    {
        public CreateMessageRequestValidator()
        {
            RuleFor(x => x.Type.ToUpper())
                .Must(type => new[] { "PUSH", "SMS", "EMAIL" }.Contains(type))
                .WithMessage("Type must be one of the following: PUSH, SMS, EMAIL."); ;

            RuleFor(x => x.ExternalClientID)
            .NotEmpty()
            .When(x => x.Type.ToUpper() == "PUSH")
            .WithMessage("ExternalClientID is required for PUSH type.");

            RuleFor(x => x.ContactValue)
            .NotEmpty()
            .Unless(x => x.Type.ToUpper() == "PUSH")
            .WithMessage("ContactValue is required when Type is not PUSH.");

            When(x => string.IsNullOrEmpty(x.TemplateCode), () =>
            {
                RuleFor(x => x.Content)
                    .NotEmpty()
                    .When(x => x.Type.ToUpper() == "SMS")
                    .WithMessage("Content is required for SMS type when TemplateCode is not provided.");

                RuleFor(x => x.Subject)
                    .NotEmpty()
                    .When(x => x.Type.ToUpper() == "EMAIL" || x.Type.ToUpper() == "PUSH")
                    .WithMessage("Subject is required for EMAIL and PUSH types when TemplateCode is not provided.");

                RuleFor(x => x.Content)
                    .NotEmpty()
                    .When(x => x.Type.ToUpper() == "EMAIL" || x.Type.ToUpper() == "PUSH")
                    .WithMessage("Content is required for EMAIL and PUSH types when TemplateCode is not provided.");
            });
        }
    }
}