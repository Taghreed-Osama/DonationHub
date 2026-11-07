using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Common.Results
{
    public sealed record RefreshTokenResult(
        string Token,
        DateTimeOffset ExpiresAt);
}
