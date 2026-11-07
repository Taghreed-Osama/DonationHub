using DonationHub.Application.Common.Results;
using DonationHub.Application.Interfaces.IIdentityService;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace DonationHub.Infrastructure.Identity
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _context;
        private readonly JwtOptions _options;
        private readonly TimeProvider _timeProvider;

        public RefreshTokenService(
            AppDbContext context,
            IOptions<JwtOptions> options,
            TimeProvider timeProvider)
        {
            _context = context;
            _options = options.Value;
            _timeProvider = timeProvider;
        }

        public async Task<RefreshTokenResult> CreateAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            var rawToken = Convert.ToBase64String(randomBytes);

            var tokenHash = Convert.ToHexString(
                SHA256.HashData(
                    Encoding.UTF8.GetBytes(rawToken)));

            var now = _timeProvider.GetUtcNow();

            var expiresAt = now.AddDays(
                _options.RefreshTokenExpiryDays);

            var refreshToken = RefreshToken.Create(
                userId,
                tokenHash,
                expiresAt,
                now);

            await _context.RefreshTokens.AddAsync(
                refreshToken,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new RefreshTokenResult(
                rawToken,
                expiresAt);
        }

        public async Task<Guid?> GetUserIdByTokenAsync(
            string rawToken,
            CancellationToken cancellationToken = default)
        {
            var tokenHash = Convert.ToHexString(
                SHA256.HashData(
                    Encoding.UTF8.GetBytes(rawToken)));

            var now = _timeProvider.GetUtcNow();

            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

            if (refreshToken is null)
                return null;

            if (refreshToken.IsExpired(now) || refreshToken.IsRevoked)
                return null;

            return refreshToken.UserId;
        }

        public async Task<RefreshTokenRotationResult?> RotationAsync(
            string rawToken,
            CancellationToken cancellationToken = default)
        {
            var tokenHash = Convert.ToHexString(
               SHA256.HashData(
                   Encoding.UTF8.GetBytes(rawToken)));

            var now = _timeProvider.GetUtcNow();

            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable, cancellationToken);

            var oldToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

            if (oldToken is null || oldToken.IsExpired(now) || oldToken.IsRevoked)
                return null;

            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var newRawToken = Convert.ToBase64String(randomBytes);

            var newTokenHash = Convert.ToHexString(
                SHA256.HashData(
                    Encoding.UTF8.GetBytes(newRawToken)));

            var expiresAt = now.AddDays(
                _options.RefreshTokenExpiryDays);

            var newToken = RefreshToken.Create(
                oldToken.UserId,
                newTokenHash,
                expiresAt,
                now);

            oldToken.Revoke(now, newToken.Id);

            await _context.RefreshTokens.AddAsync(
                newToken,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new RefreshTokenRotationResult(
                oldToken.UserId,
                newRawToken,
                expiresAt);
        }

        public async Task<bool> RevokeAsync(
            string rawToken,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(rawToken))
                return false;

            var tokenHash = Convert.ToHexString(
                SHA256.HashData(
                    Encoding.UTF8.GetBytes(rawToken)));

            var refreshToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

            if(refreshToken is null)
                return false;

            if(refreshToken.IsRevoked)
                return false;

            var now = _timeProvider.GetUtcNow();

            refreshToken.Revoke(now);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
