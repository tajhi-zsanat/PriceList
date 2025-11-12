using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PriceList.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Auth
{
    public class TokenService(IConfiguration cfg) : ITokenService
    {
        public string CreateAccessToken(AppUser user, IList<string> roles, IList<Claim> userClaims)
        {
            var jwt = cfg.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
            claims.AddRange(userClaims);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["AccessTokenMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string token, DateTime expiresAt) CreateRefreshToken()
        {
            var rng = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var exp = DateTime.UtcNow.AddDays(int.Parse(cfg["Jwt:RefreshTokenDays"]!));
            return (rng, exp);
        }
    }
}
