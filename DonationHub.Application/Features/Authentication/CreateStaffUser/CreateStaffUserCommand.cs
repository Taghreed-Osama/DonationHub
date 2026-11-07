using DonationHub.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.CreateStaffUser
{
    public sealed record CreateStaffUserCommand(
        string Email,
        string FullName,
        string Password,
        string PreferredLanguage
        ) : IRequest<IdentityOperationResult>;
}
