using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DonationHub.Infrastructure.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        [Required]
        public string SecretKey { get; set; } = null!;

        [Required]
        public string Issuer { get; set; } = null!;

        [Required]
        public string Audience { get; set; } = null!;

        [Range(1, 15)]
        public int ExpiryMinutes { get; set; }

        [Range(1, 7)]
        public int RefreshTokenExpiryDays { get; set; }
    }
}
