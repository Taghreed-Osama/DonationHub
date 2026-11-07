using DonationHub.Application.Common.Results;
using DonationHub.Application.Interfaces.IIdentityService;
using DonationHub.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DonationHub.Infrastructure.Identity
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _options;
        private readonly TimeProvider _timeProvider;

        public JwtTokenService(
            IOptions<JwtOptions> options,
            TimeProvider timeProvider)
        {
            _options = options.Value;
            _timeProvider = timeProvider;
        }

        public AccessTokenResult GenerateAccessToken(
            Guid userId,
            string email,
            IReadOnlyCollection<string> roles)
        {
            var now = _timeProvider.GetUtcNow();

            var expiresAt = now.AddMinutes(
                _options.ExpiryMinutes);

            var claims = new List<Claim>
        {
            new("UserId", userId.ToString()),

            new(ClaimTypes.NameIdentifier, userId.ToString()),

            new(ClaimTypes.Email, email)
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var keyBytes = Convert.FromBase64String(_options.SecretKey);

            var key = new SymmetricSecurityKey(keyBytes);

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now.UtcDateTime,
                expires: expiresAt.UtcDateTime,
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return new AccessTokenResult(
                tokenString,
                expiresAt);
        }

    }
}
