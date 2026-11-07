using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Common.Results
{
    public sealed record IdentityLoginResult(
        bool Succeeded,
        bool AccountDeactivated,
        Guid? UserId,
        string? Email,
        string? FullName,
        string? PreferredLanguage,
        IReadOnlyList<string> Roles);
}
