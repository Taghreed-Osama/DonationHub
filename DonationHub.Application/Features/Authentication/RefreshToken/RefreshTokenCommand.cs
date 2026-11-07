using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.RefreshToken
{
    public sealed record RefreshTokenCommand(
        string RefreshToken
        ) : IRequest<RefreshTokenResponse>;
}
