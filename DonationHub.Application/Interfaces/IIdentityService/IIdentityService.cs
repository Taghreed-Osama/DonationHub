using DonationHub.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Interfaces.IIdentityService
{
    public interface IIdentityService
    {
        Task<IdentityOperationResult> RegisterDonorAsync(
            string email,
            string fullname,
            string password,
            string preferredlanguage
            );

        Task<IdentityOperationResult> CreateStaffUserAsync(
            string email,
            string fullname,
            string password,
            string preferredlanguage);

        Task<IdentityLoginResult> AuthenticateAsync(
            string email,
            string password
            );

        Task<RefreshUserResult> GetUserForRefreshAsync(
            Guid userId,
            CancellationToken cancellationToken = default
            );

        Task<CurrentUserResult?> GetCurrentUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default
            );
    }
}
