using DonationHub.Domain.Common;
using DonationHub.Domain.Entities;
using DonationHub.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = null!;
        public string PreferredLanguage { get; set; } = "en";
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        private readonly List<Campaign> _campaigns = new();
        public IReadOnlyCollection<Campaign> Campaigns => _campaigns;

        private readonly List<Donation> _donations = new();
        public IReadOnlyCollection<Donation> Donations => _donations;

        private ApplicationUser() { }

        public ApplicationUser(
            string fullname,
            string preferredlanguage,
            bool isActive,
            DateTimeOffset now)
            => (FullName, PreferredLanguage, IsActive, CreatedAt)
            = (fullname, preferredlanguage, isActive, now);
    }
}
