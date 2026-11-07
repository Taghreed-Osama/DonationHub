using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Logout
{
    public sealed record LogoutCommand(
        string RefreshToken) : IRequest<Unit>;
}
