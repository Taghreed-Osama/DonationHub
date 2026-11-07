using DonationHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class IdempotencyRecord : BaseEntity
    {
        public string Key { get; private set; } = null!;
        public Guid UserId { get; private set; }
        public string RequestHash { get; private set; } = null!;
        public int ResponseStatusCode { get; private set; }
        public string ResponseBody { get; private set; } = null!;
        public DateTimeOffset ExpiresAt { get; private set; }

        private IdempotencyRecord() { }

        public IdempotencyRecord(string key, Guid userId, string requestHash, int responseStatusCode, string responseBody, DateTimeOffset expiresAt, DateTimeOffset now) : base(now)
        {
            Key = key;
            UserId = userId;
            RequestHash = requestHash;
            ResponseStatusCode = responseStatusCode;
            ResponseBody = responseBody;
            ExpiresAt = expiresAt;
        }
    }
}
