using DonationHub.Application.Common.Results;
using DonationHub.Application.Exceptions;
using DonationHub.Application.Interfaces.IIdentityService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Me
{
    public class GetCurrentUserQueryHandler
        :IRequestHandler<GetCurrentUserQuery, CurrentUserResult>
    {
        private readonly IIdentityService _identityService;
        public GetCurrentUserQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<CurrentUserResult> Handle(
            GetCurrentUserQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _identityService.GetCurrentUserAsync(
                request.UserId, cancellationToken);

            if (user is null)
                throw new UnauthorizedException("Auth.UserNotFound");

            return user;
        }
    }
}
