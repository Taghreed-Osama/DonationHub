using DonationHub.Application.Exceptions;
using DonationHub.Application.Interfaces.IIdentityService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginCommandHandler(
            IIdentityService identityService,
            IJwtTokenService jwtTokenService,
            IRefreshTokenService refreshTokenService)
        {
            _identityService = identityService;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LoginResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var result = await _identityService.AuthenticateAsync(
                request.Email,
                request.Password);

            if (!result.Succeeded && !result.AccountDeactivated)
            {
                throw new UnauthorizedException(
                    "Auth.InvalidCredentials");
            }

            if (result.AccountDeactivated)
            {
                throw new ForbiddenException(
                    "Auth.AccountDeactivated");
            }

            var accessToken = _jwtTokenService.GenerateAccessToken(
                result.UserId!.Value,
                result.Email!,
                result.Roles);

            var refreshToken = await _refreshTokenService.CreateAsync(
                result.UserId.Value,
                cancellationToken);

            return new LoginResponse(
                accessToken.Token,
                accessToken.ExpiresAt,
                refreshToken.Token,
                refreshToken.ExpiresAt,
                new UserInfo(
                    result.UserId.Value,
                    result.Email!,
                    result.FullName!,
                    result.PreferredLanguage!,
                    result.Roles));
        }
    }
}
