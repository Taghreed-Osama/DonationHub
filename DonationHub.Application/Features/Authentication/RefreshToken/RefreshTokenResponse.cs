using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.RefreshToken
{
    public sealed record RefreshTokenResponse(
        string AccessToken,
        DateTimeOffset AccessTokenExpiresAt,
        string RefreshToken,
        DateTimeOffset RefreshTokenExpiresAt);
}
