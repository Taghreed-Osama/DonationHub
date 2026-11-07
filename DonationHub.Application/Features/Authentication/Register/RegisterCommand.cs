using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Register
{
    public sealed record RegisterCommand(
        string Email,
        string FullName,
        string Password,
        string? PreferredLanguage
        ) : IRequest<RegisterResponse>;
}
