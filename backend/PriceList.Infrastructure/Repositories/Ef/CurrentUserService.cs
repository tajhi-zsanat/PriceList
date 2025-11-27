using Microsoft.AspNetCore.Http;
using PriceList.Core.Abstractions.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user is null || !user.Identity?.IsAuthenticated == true)
                return null;

            // Must match what you wrote in the token
            var idClaim =
                user.FindFirst(ClaimTypes.NameIdentifier) ?? // if you used NameIdentifier
                user.FindFirst(JwtRegisteredClaimNames.Sub) ?? // if you used "sub"
                user.FindFirst("userId"); // if you used custom "userId"

            return int.TryParse(idClaim?.Value, out var id)
                ? id
                : (int?)null;
        }
    }

    public string? UserName =>
        _httpContextAccessor.HttpContext?.User?.Identity?.Name;

    public string? IpAddress =>
        _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

    public string? UserAgent =>
        _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
}
