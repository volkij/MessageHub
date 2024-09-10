using FluentValidation;
using MessageHub.Core.Config;

namespace MessageHub.Core.Validators
{
    public class MessageHubConfigValidator : AbstractValidator<MessageHubConfig>
    {
        public MessageHubConfigValidator()
        {
            RuleFor(x => x.MessageRetentionDays).NotEmpty();

            RuleFor(config => config.MessageRetentionDays)
                .GreaterThan(0)
                .WithMessage("MessageRetentionDays must be greater than 0");
        }
    }
}