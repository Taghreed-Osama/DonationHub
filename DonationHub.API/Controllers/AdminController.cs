using DonationHub.API.Contract.Authentication;
using DonationHub.Application.Common.Authorization;
using DonationHub.Application.Features.Authentication.CreateStaffUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin")]
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("staff")]
        public async Task<IActionResult> CreateStaffUser(
            [FromBody] CreateStaffUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateStaffUserCommand(
                request.Email,
                request.FullName,
                request.Password,
                request.PreferredLanguage);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (!result.Succeeded)
            {
                if (result.EmailAlreadyExists)
                    return Conflict(new
                    {
                        Code = "Identity.DuplicateEmail",
                        Message = "Email is already registered."
                    });

                return BadRequest(new
                {
                    Code = "Identity.UserCreationFailed",
                    Errors = result.Errors
                });
            }

            return StatusCode(StatusCodes.Status201Created, new
            {
                UserId = result.UserId,
                Message = "Staff user created successfully."
            });
        }
    }
}