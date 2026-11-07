using DonationHub.Application.Exceptions;
using DonationHub.Application.Interfaces.IIdentityService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IIdentityService _identityService;

        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<RegisterResponse> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim();

            var fullName = request.FullName.Trim();

            var preferredLanguage =
                string.IsNullOrWhiteSpace(request.PreferredLanguage)
                ? "en" : request.PreferredLanguage.Trim().ToLowerInvariant();

            var result = await _identityService.RegisterDonorAsync(
                email,
                fullName,
                request.Password,
                preferredLanguage);

            if (result.EmailAlreadyExists)
            {
                throw new ConflictException("Auth.EmailAlreadyExist");
            }

            if (!result.Succeeded)
            {
                throw new ValidationException(string.Join(", ", result.Errors));
            }

            return new RegisterResponse(
                result.UserId!.Value,
                email,
                fullName,
                preferredLanguage
                );
        }
    }
}
