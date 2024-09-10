using FluentValidation;
using MessageHub.Core.Config;


namespace MessageHub.Core.Validations
{
    public class AccountConfigValidator : AbstractValidator<AccountConfig>
    {
        public AccountConfigValidator()
        {
            RuleForEach(x => x.Accounts).ChildRules(accounts =>
            {
                accounts.RuleFor(a => a.Name).NotEmpty();
                accounts.RuleFor(a => a.ApiKey).NotEmpty();
            });


            RuleFor(x => x.Accounts)
                   .Must(accounts => accounts.Select(a => a.Name).Distinct().Count() == accounts.Count)
                   .WithMessage("Account names must be unique.");
        }
    }
}