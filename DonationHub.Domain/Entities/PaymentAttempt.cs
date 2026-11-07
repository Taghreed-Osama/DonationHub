using DonationHub.Domain.Common;
using DonationHub.Domain.Enums;
using DonationHub.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class PaymentAttempt : BaseEntity
    {
        public Guid DonationId { get; private set; }
        public string StripeCheckoutSessionId { get; private set; } = null!;
        public string? StripePaymentIntentId { get; private set; }
        public Money Amount { get; private set; } = null!;
        public PaymentAttemptStatus Status { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }
        public string? FailureCode { get; private set; }

        private PaymentAttempt() { }
        public PaymentAttempt(Guid donationId, string stripeCheckoutSessionId, string? stripePaymentIntentId, Money amount, DateTimeOffset now) : base(now)
        {
            DonationId = donationId;
            StripeCheckoutSessionId = stripeCheckoutSessionId;
            StripePaymentIntentId = stripePaymentIntentId;
            Amount = amount;
            Status = PaymentAttemptStatus.Created;
            UpdatedAt = now;
        }

        public void MarkAsSucceeded(DateTimeOffset now)
        {
            if (Status != PaymentAttemptStatus.Created)
            {
                throw new InvalidOperationException("Can't Change this status.");
            }
            Status = PaymentAttemptStatus.Succeeded;
            UpdatedAt = now;
        }

        public void MarkAsFailed(DateTimeOffset now)
        {
            if (Status != PaymentAttemptStatus.Created)
            {
                throw new InvalidOperationException("Can't Change this status.");
            }
            Status = PaymentAttemptStatus.Failed;
            UpdatedAt = now;
        }

        public void MarkAsExpired(DateTimeOffset now)
        {
            if (Status != PaymentAttemptStatus.Created)
            {
                throw new InvalidOperationException("Can't Change this status.");
            }
            Status = PaymentAttemptStatus.Expired;
            UpdatedAt = now;
        }
    }
}
