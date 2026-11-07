using DonationHub.API.Contract.Authentication;
using DonationHub.Application.Features.Authentication.Login;
using DonationHub.Application.Features.Authentication.Logout;
using DonationHub.Application.Features.Authentication.RefreshToken;
using DonationHub.Application.Features.Authentication.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Security.Claims;
using DonationHub.Application.Features.Authentication.Me;
using DonationHub.Application.Exceptions;

namespace DonationHub.API.Controllers
{
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMediator _mediator;

        public AuthController(
            ISender sender,
            IMediator mediator)
        {
            _sender = sender;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(
            RegisterCommand command,
            CancellationToken cancellationToken)
        {
            var response = await _sender.Send(command, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                command,
                cancellationToken);

            Response.Cookies.Append(
                "refreshToken",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = result.RefreshTokenExpiresAt,
                    Path = "/auth"
                });

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            var command = new RefreshTokenCommand(request.RefreshToken);

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            [FromBody] LogoutRequest request,
            CancellationToken cancellationToken)
        {
            var command = new LogoutCommand(request.RefreshToken);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me(
            CancellationToken cancellationToken)
        {
            var userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if(!Guid.TryParse(userIdValue, out var userId))
            {
                throw new UnauthorizedException("Auth.InvalidToken");
            }

            var query = new GetCurrentUserQuery(userId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}
