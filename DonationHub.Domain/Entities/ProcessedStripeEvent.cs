using DonationHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class ProcessedStripeEvent : BaseEntity
    {
        public Guid EventId { get; private set; }
        public string Type { get; private set; } = null!;
        public DateTimeOffset ProcessedAt { get; private set; }

        private ProcessedStripeEvent() { }

        public ProcessedStripeEvent(Guid eventId, string type, DateTimeOffset processedAt, DateTimeOffset now) : base(now)
        {
            EventId = eventId;
            Type = type;
            ProcessedAt = processedAt;
        }
    }
}
