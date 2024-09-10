﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MessageHub.Domain.Validators
{
    public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string? _name;

        public FluentValidationOptions(string? name, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _name = name;
        }

        public ValidateOptionsResult Validate(string? name, TOptions options)
        {
            // Null name is used to configure all named options.
            if (_name != null && _name != name)
            {
                // Ignored if not validating this instance.
                return ValidateOptionsResult.Skip;
            }

            // Ensure options are provided to validate against
            ArgumentNullException.ThrowIfNull(options);

            // Validators are registered as scoped, so need to create a scope,
            // as we will be called from the root scope
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();
            var results = validator.Validate(options);
            if (results.IsValid)
            {
                return ValidateOptionsResult.Success;
            }

            string typeName = options.GetType().Name;
            var errors = new List<string>();
            foreach (var result in results.Errors)
            {
                errors.Add($"Fluent validation failed for '{typeName}.{result.PropertyName}' with the error: '{result.ErrorMessage}'.");
            }

            return ValidateOptionsResult.Fail(errors);
        }
    }

    public static class OptionsBuilderFluentValidationExtensions
    {
        public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
        {
            optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
                provider => new FluentValidationOptions<TOptions>(optionsBuilder.Name, provider));
            return optionsBuilder;
        }
    }
}