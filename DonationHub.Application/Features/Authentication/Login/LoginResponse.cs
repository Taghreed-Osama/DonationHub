
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Login
{
    public sealed record LoginResponse(
        string AccessToken,
        DateTimeOffset AccessTokenExpiresAt,
        string RefreshToken,
        DateTimeOffset RefreshTokenExpiresAt,
        UserInfo User);

    public sealed record UserInfo(
        Guid Id,
        string Email,
        string FullName,
        string PreferredLanguage,
        IReadOnlyList<string> Roles);
}
