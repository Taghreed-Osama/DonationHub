using DonationHub.Application.Common.Results;
using DonationHub.Application.Interfaces.IIdentityService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.CreateStaffUser
{
    public sealed class CreateStaffUserCommandHandler
        : IRequestHandler<CreateStaffUserCommand, IdentityOperationResult>
    {
        private readonly IIdentityService _identityService;

        public CreateStaffUserCommandHandler(
            IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<IdentityOperationResult> Handle(
            CreateStaffUserCommand request,
            CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateStaffUserAsync(
                request.Email,
                request.FullName,
                request.Password,
                request.PreferredLanguage);

            return result;
        }
    }
}
