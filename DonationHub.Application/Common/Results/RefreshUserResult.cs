using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Common.Results
{
    public sealed record RefreshUserResult(
    bool Exists,
    bool IsActive,
    Guid UserId,
    string Email,
    IReadOnlyList<string> Roles);
}
