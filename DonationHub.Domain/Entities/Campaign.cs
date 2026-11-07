using DonationHub.Domain.Common;
using DonationHub.Domain.Contract;
using DonationHub.Domain.Enums;
using DonationHub.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class Campaign : BaseEntity, ISoftDeletable
    {
        public string TitleAr { get; private set; } = null!;
        public string TitleEn { get; private set; } = null!;
        public string DescriptionAr { get; private set; } = null!;
        public string DescriptionEn { get; private set; } = null!;
        public Money GoalAmount { get; private set; } = null!;
        public Money RaisedAmount { get; private set; } = null!;
        public DateTimeOffset EndDate { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;
        public string? BannerFileName { get; private set; }
        public CampaignStatus Status { get; private set; }
        public Guid CreatedByUserId { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }
        public byte[] RowVersion { get; private set; } = [];

        private readonly List<Donation> _donations = new();
        public IReadOnlyCollection<Donation> Donations => _donations;

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }

        private Campaign() { }

        public Campaign(
            string titlear,
            string titleen,
            string descriptionar,
            string descriptionen,
            Money goalamount,
            Money raisedamount,
            DateTimeOffset enddate,
            Guid categoryid,
            string? bannerfilename,
            Guid createdbyuserid,
            DateTimeOffset now) : base(now)
            => (TitleAr, TitleEn, DescriptionAr, DescriptionEn, GoalAmount, RaisedAmount, EndDate, CategoryId, BannerFileName, CreatedByUserId, UpdatedAt, Status)
            = (titlear, titleen, descriptionar, descriptionen, goalamount, raisedamount, enddate, categoryid, bannerfilename, createdbyuserid, now, CampaignStatus.Active);



        public void ValidGoalAmount(Money goalAmount, DateTimeOffset now)
        {
            if (goalAmount.Amount <= 0)
            {
                throw new ArgumentOutOfRangeException("Goal amount must be grater than 0.", nameof(goalAmount));
            }

            if (GoalAmount.Amount < RaisedAmount.Amount)
            {
                throw new InvalidOperationException("Goal amount can't be less than Raised amount.");
            }
            GoalAmount = goalAmount;
            UpdatedAt = now;
        }

        public void ValidEndDate(DateTimeOffset endDate, DateTimeOffset now)
        {
            if (endDate <= now)
            {
                throw new ArgumentException("End date must be in the future.", nameof(endDate));
            }
            EndDate = endDate;
            UpdatedAt = now;
        }

        public bool CanAcceptDonations(Money amount, DateTimeOffset now)
        {
            if (Status == CampaignStatus.Active && EndDate > now && !IsDeleted)
            {
                if (RaisedAmount.Amount < GoalAmount.Amount)
                {
                    RaisedAmount.Amount += amount.Amount;
                }
                else
                {
                    Status = CampaignStatus.Completed;
                }
                UpdatedAt = now;
                return true;
            }
            return false;
        }

        public void MarkAsCompleted(DateTimeOffset now)
        {
            if (Status != CampaignStatus.Active)
            {
                throw new InvalidOperationException("Can't Change this status.");
            }
            Status = CampaignStatus.Completed;
            UpdatedAt = now;
        }

        public void MarkAsExpired(DateTimeOffset now)
        {
            if (EndDate < now)
            {
                throw new InvalidOperationException("Expire date hasn't came yet.");
            }
            if (Status != CampaignStatus.Active)
            {
                throw new InvalidOperationException("Can't Change this status.");
            }
            Status = CampaignStatus.Expired;
            UpdatedAt = now;
        }
    }
}
