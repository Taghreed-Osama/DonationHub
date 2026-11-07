using DonationHub.Application.Exceptions;
using DonationHub.Application.Interfaces.IIdentityService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.RefreshToken
{
    public sealed class RefreshTokenCommandHandler
        : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenCommandHandler(
            IRefreshTokenService refreshTokenService,
            IIdentityService identityService,
            IJwtTokenService jwtTokenService)
        {
            _refreshTokenService = refreshTokenService;
            _identityService = identityService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<RefreshTokenResponse> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var userId = await _refreshTokenService.GetUserIdByTokenAsync(
                request.RefreshToken,
                cancellationToken);

            if (userId is null)
                throw new UnauthorizedException("Auth.RefreshTokenInvalid");

            var user = await _identityService.GetUserForRefreshAsync(
                userId.Value,
                cancellationToken);

            if(!user.Exists)
                throw new UnauthorizedException("Auth.RefreshTokenInvalid");

            if (!user.IsActive)
                throw new InvalidOperationException("Auth.AccountDeactivated");

            var rotationResult = await _refreshTokenService.RotationAsync(
                request.RefreshToken,
                cancellationToken);

            if (rotationResult is null)
                throw new UnauthorizedException("Auth.RefreshTokenInvalid");

            var accessToken = _jwtTokenService.GenerateAccessToken(
                user.UserId,
                user.Email,
                user.Roles);

            return new RefreshTokenResponse(
                accessToken.Token,
                accessToken.ExpiresAt,
                rotationResult.RefreshToken,
                rotationResult.ExpiresAt);
        }
    }
}
