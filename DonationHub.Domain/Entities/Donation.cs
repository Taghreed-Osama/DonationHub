using DonationHub.Domain.Common;
using DonationHub.Domain.Enums;
using DonationHub.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class Donation : BaseEntity
    {
        public Guid CampaignId { get; private set; }
        public Campaign Campaign { get; private set; } = null!;
        public Guid DonorUserId { get; private set; }
        public Money Amount { get; private set; } = null!;
        public DonationStatus Status { get; private set; }
        public DateTimeOffset? CompletedAt { get; private set; }
        public string? FailureReason { get; private set; }
        public int? ReceiptNumber { get; private set; }
        public string? ReceiptFileName { get; private set; }

        private Donation() { }

        public Donation(Guid campaignId, Guid donorUserId, Money amount, DateTimeOffset now) : base(now)
        {
            CampaignId = campaignId;
            DonorUserId = donorUserId;
            Amount = amount;
            Status = DonationStatus.Pendeing;
        }

        public void MarkAsSucceeded()
        {
            if (Status != DonationStatus.Pendeing)
            {
                throw new InvalidOperationException("Can't Change this status.");
            }
            Status = DonationStatus.Succeeded;
        }

        public void MarkAsFailed()
        {
            if (Status != DonationStatus.Pendeing)
            {
                throw new InvalidOperationException("Can't Change this status.");
            }
            Status = DonationStatus.Failed;
        }
    }
}
