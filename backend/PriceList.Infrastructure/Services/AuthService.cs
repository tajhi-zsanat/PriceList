using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PriceList.Core.Application.Dtos.Auth;
using PriceList.Core.Application.Services;
using PriceList.Infrastructure.Auth;
using PriceList.Infrastructure.Data;
using PriceList.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Services
{
    public class AuthService(
     UserManager<AppUser> userManager,
     SignInManager<AppUser> signInManager,
     ITokenService tokens,
     AppDbContext db,
     IConfiguration cfg) : IAuthService
    {
        public async Task<AuthResult> RegisterAsync(RegisterDto dto, CancellationToken ct)
        {
            var user = new AppUser { UserName = dto.UserName, Email = dto.Email, DisplayName = dto.DisplayName };
            var result = await userManager.CreateAsync(user, dto.Password);
            return result.Succeeded
                ? new AuthResult(true, Array.Empty<string>())
                : new AuthResult(false, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<LoginResult> LoginAsync(LoginDto dto, AuthRequestInfo req, CancellationToken ct)
        {
            var userAgent = req.UserAgent;
            var ip = req.Ip;

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName, ct);
            if (user is null) return new LoginResult(false, "", null, null, null!, "", DateTime.MinValue);

            var pwd = await signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
            if (!pwd.Succeeded) return new LoginResult(false, "", null, null, null!, "", DateTime.MinValue);

            var roles = await userManager.GetRolesAsync(user);
            var claims = await userManager.GetClaimsAsync(user);

            var access = tokens.CreateAccessToken(user, roles, claims);
            var (refresh, exp) = tokens.CreateRefreshToken();

            // store hashed token
            var hash = Hash(refresh);
            db.RefreshTokens.Add(new RefreshToken
            {
                Token = hash,
                ExpiresAtUtc = exp,
                UserId = user.Id,
                UserAgent = userAgent,
                Ip = ip
            });
            await db.SaveChangesAsync(ct);

            var userPayload = new { user.Id, user.UserName, user.DisplayName, roles };
            return new LoginResult(true, access, user.Id, user.UserName, userPayload, refresh, exp);
        }

        public async Task<RefreshResult> RefreshAsync(string refreshCookieValue, AuthRequestInfo req, CancellationToken ct)
        {
            var userAgent = req.UserAgent;
            var ip = req.Ip;

            var hash = Hash(refreshCookieValue);

            var entry = await db.RefreshTokens.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == hash, ct);

            if (entry is null || entry.Revoked || entry.ExpiresAtUtc < DateTime.UtcNow)
                return new RefreshResult(false, "", "", DateTime.MinValue);

            // rotate
            entry.Revoked = true;

            var (newRefresh, newExp) = tokens.CreateRefreshToken();
            db.RefreshTokens.Add(new RefreshToken
            {
                Token = Hash(newRefresh),
                ExpiresAtUtc = newExp,
                UserId = entry.UserId,
                UserAgent = userAgent,
                Ip = ip
            });

            var roles = await userManager.GetRolesAsync(entry.User);
            var claims = await userManager.GetClaimsAsync(entry.User);
            var access = tokens.CreateAccessToken(entry.User, roles, claims);

            await db.SaveChangesAsync(ct);

            return new RefreshResult(true, access, newRefresh, newExp);
        }

        public async Task LogoutAsync(string refreshCookieValue, CancellationToken ct)
        {
            var hash = Hash(refreshCookieValue);
            var entry = await db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == hash, ct);
            if (entry != null)
            {
                entry.Revoked = true;
                await db.SaveChangesAsync(ct);
            }
        }

        static string Hash(string input)
        {
            // SHA256 + base64; store only the hash in DB
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
