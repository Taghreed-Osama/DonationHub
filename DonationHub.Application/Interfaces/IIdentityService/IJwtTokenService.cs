using DonationHub.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Interfaces.IIdentityService
{
    public interface IJwtTokenService
    {
        AccessTokenResult GenerateAccessToken(
            Guid Id,
            string email,
            IReadOnlyCollection<string> roles);
    }
}
