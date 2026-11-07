using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Login
{
    public sealed record LoginCommand(
        string Email,
        string Password
    ) : IRequest<LoginResponse>;
}
