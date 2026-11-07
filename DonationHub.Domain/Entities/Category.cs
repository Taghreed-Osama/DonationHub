using DonationHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string NameAr { get; private set; } = null!;
        public string NameEn { get; private set; } = null!;
        public string DescriptionAr { get; private set; } = null!;
        public string DescriptionEn { get; private set; } = null!;
        public DateTimeOffset UpdatedAt { get; private set; }

        private readonly List<Campaign> _campaigns = new();
        public IReadOnlyCollection<Campaign> Campaigns => _campaigns;

        private Category() { }

        public Category(string nameAr, string nameEn, string descriptionAr, string descriptionEn, DateTimeOffset now) : base(now)
        {
            NameAr = nameAr;
            NameEn = nameEn;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            UpdatedAt = now;
        }
    }

}
