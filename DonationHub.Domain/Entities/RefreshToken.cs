using DonationHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string TokenHash { get; private set; } = null!;
        public DateTimeOffset ExpiresAt { get; private set; }
        public DateTimeOffset? RevokedAt { get; private set; }
        public Guid? ReplacedByTokenId { get; private set; }

        private RefreshToken() { }

        private RefreshToken(Guid userId, string tokenHash, DateTimeOffset expiresAt, DateTimeOffset now) : base(now)
        {
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
        }

        public static RefreshToken Create(
            Guid userId,
            string tokenHash,
            DateTimeOffset expiresAt,
            DateTimeOffset now)
        {
            return new RefreshToken
            (
                userId,
                tokenHash,
                expiresAt,
                now
            );
        }

        public bool IsExpired(DateTimeOffset now)
        {
            return ExpiresAt <= now;
        }

        public bool IsRevoked => RevokedAt.HasValue;

        public void Revoke(
            DateTimeOffset revokedAt,
            Guid? replacedByTokenId = null)
        {
            if (IsRevoked)
                throw new InvalidOperationException("Refresh token has already been revoked.");

            RevokedAt = revokedAt;
            ReplacedByTokenId = replacedByTokenId;
        }
    }
}
