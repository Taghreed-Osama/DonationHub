using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Common.Results
{
    public sealed record IdentityOperationResult(
        bool Succeeded,
        Guid? UserId,
        bool EmailAlreadyExists,
        IReadOnlyList<string> Errors
        );
}
