using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Options
{
    public sealed class DevelopmentUsersOptions
    {
        public const string SectionName = "DevelopmentUsers";

        public TestUserOptions CampaignManager { get; set; } = new();

        public TestUserOptions Donor { get; set; } = new();
    }

    public sealed class TestUserOptions
    {
        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string PreferredLanguage { get; set; } = "en";
    }
}
