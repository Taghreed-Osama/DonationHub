using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.PreferredLanguage)
                .Must(language =>
                    string.IsNullOrWhiteSpace(language) ||
                    language.Trim().ToLowerInvariant() is "en" or "ar")
                .WithMessage("Preferred Language must be en or ar");
        }
    }
}
