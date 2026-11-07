using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Common.Results
{
    public sealed record RefreshTokenRotationResult(
        Guid userId,
        string RefreshToken,
        DateTimeOffset ExpiresAt);
}
