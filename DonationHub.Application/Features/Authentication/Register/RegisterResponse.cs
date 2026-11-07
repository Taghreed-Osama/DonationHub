using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Register
{
    public sealed record RegisterResponse(
        Guid UserId,
        string Email,
        string FullName,
        string PreferredLanguage);
}
