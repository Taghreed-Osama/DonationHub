using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Common.Results
{
    public sealed record CurrentUserResult(
        Guid Id,
        string Email,
        string FullName,
        string preferredLanguaue,
        IReadOnlyList<string> Roles);
}
