using DonationHub.Application.Common.Authorization;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Interfaces.IIdentityService;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TimeProvider _timeProvider;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            TimeProvider timeProvider)
        {
            _userManager = userManager;
            _timeProvider = timeProvider;
        }

        public async Task<IdentityOperationResult> RegisterDonorAsync(
            string email,
            string fullname,
            string password,
            string preferredlanguage)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is not null)
            {
                return new IdentityOperationResult(
                    false,
                    null,
                    true,
                    Array.Empty<string>());
            }

            var user = new ApplicationUser(
                fullname, preferredlanguage, true, _timeProvider.GetUtcNow())
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserName = email
            };


            var createResult = await _userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                var dublicateEmail = createResult.Errors
                    .Any(error =>
                    error.Code == "DuplicateEmail" ||
                    error.Code == "DuplicateUserName");

                return new IdentityOperationResult(
                    false,
                    null,
                    dublicateEmail,
                    createResult.Errors
                        .Select(error => error.Code)
                        .ToArray());
            }


            var roleResult = await _userManager.AddToRoleAsync(
                user,
                Roles.Donor);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return new IdentityOperationResult(
                    false,
                    null,
                    false,
                    roleResult.Errors
                        .Select(error => error.Code)
                        .ToArray());
            }

            return new IdentityOperationResult(
                true,
                user.Id,
                false,
                Array.Empty<string>());
        }

        public async Task<IdentityOperationResult> CreateStaffUserAsync(
            string email,
            string fullname,
            string password,
            string preferredlanguage)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is not null)
            {
                return new IdentityOperationResult(
                    false,
                    null,
                    true,
                    Array.Empty<string>());
            }

            var user = new ApplicationUser(
                fullname,
                preferredlanguage,
                true,
                _timeProvider.GetUtcNow())
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserName = email
            };

            var createResult = await _userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                var duplicateEmail = createResult.Errors.Any(error =>
                    error.Code == "DuplicateEmail" ||
                    error.Code == "DuplicateUserName");

                return new IdentityOperationResult(
                    false,
                    null,
                    duplicateEmail,
                    createResult.Errors.Select(error => error.Code).ToArray());
            }

            var roleResult = await _userManager.AddToRoleAsync(
                user,
                Roles.CampaignManager);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return new IdentityOperationResult(
                    false,
                    null,
                    false,
                    roleResult.Errors.Select(error => error.Code).ToArray());
            }

            return new IdentityOperationResult(
                true,
                user.Id,
                false,
                Array.Empty<string>());
        }

        public async Task<IdentityLoginResult> AuthenticateAsync(
            string email,
            string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return new IdentityLoginResult(
                    false,
                    false,
                    null,
                    null,
                    null,
                    null,
                    Array.Empty<string>());
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, password);

            if (!passwordValid)
            {
                return new IdentityLoginResult(
                    false,
                    false,
                    null,
                    null,
                    null,
                    null,
                    Array.Empty<string>());
            }

            if (!user.IsActive)
            {
                return new IdentityLoginResult(
                    false,
                    true,
                    null,
                    null,
                    null,
                    null,
                    Array.Empty<string>());
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new IdentityLoginResult(
                true,
                false,
                user.Id,
                user.Email!,
                user.FullName,
                user.PreferredLanguage,
                roles.ToArray());
        }

        public async Task<RefreshUserResult> GetUserForRefreshAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return new RefreshUserResult(
                    false,
                    false,
                    userId,
                    string.Empty,
                    Array.Empty<string>());
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new RefreshUserResult(
                true,
                user.IsActive,
                user.Id,
                user.Email ?? string.Empty,
                roles.ToArray());
        }

        public async Task<CurrentUserResult?> GetCurrentUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new CurrentUserResult(
                user.Id,
                user.Email?? string.Empty,
                user.FullName,
                user.PreferredLanguage,
                roles.ToArray());
        }
    }
}
