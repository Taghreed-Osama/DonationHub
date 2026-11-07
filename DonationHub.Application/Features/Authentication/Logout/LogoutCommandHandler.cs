using DonationHub.Application.Exceptions;
using DonationHub.Application.Interfaces.IIdentityService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Logout
{
    public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public LogoutCommandHandler(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Unit> Handle(
            LogoutCommand request,
            CancellationToken cancellationToken)
        {
            var revoked = await _refreshTokenService.RevokeAsync(
                request.RefreshToken, cancellationToken);

            if (!revoked)
                throw new UnauthorizedException("Auth.RefreshTokenInvalid");

            return Unit.Value;
        }
    }
}
