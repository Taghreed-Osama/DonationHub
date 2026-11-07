using DonationHub.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Interfaces.IIdentityService
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenResult> CreateAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<Guid?> GetUserIdByTokenAsync(
            string rawToken,
            CancellationToken cancellationToken = default);

        Task<RefreshTokenRotationResult?> RotationAsync(
            string rawToken,
            CancellationToken cancellationToken = default);

        Task<bool> RevokeAsync(
            string rawToken,
            CancellationToken cancellationToken = default);
    }
}
