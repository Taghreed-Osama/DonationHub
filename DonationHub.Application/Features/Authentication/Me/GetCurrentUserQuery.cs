using DonationHub.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Me
{
    public sealed record GetCurrentUserQuery(
        Guid UserId) : IRequest<CurrentUserResult>;
}
