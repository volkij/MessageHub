using FluentValidation;
using MessageHub.Shared;

namespace MessageHub.Core.Validators
{
    public class SenderConfigValidator : AbstractValidator<List<SenderConfig>>
    {
        public SenderConfigValidator()
        {
            RuleForEach(x => x).ChildRules(sender =>
            {
                sender.RuleFor(a => a.Code).NotEmpty();
                sender.RuleFor(a => a.Type).NotEmpty();
                sender.RuleFor(a => a.ClassName).NotEmpty();
                sender.RuleFor(a => a.AssemblyName).NotEmpty();
            });
        }
    }
}
