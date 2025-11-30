// PriceList.Api/Controllers/AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Auth;
using PriceList.Core.Application.Services;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Identity;
using PriceList.Infrastructure.Repositories.Ef;
using System.ComponentModel.DataAnnotations;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class AuthController(IAuthService auth, IUnitOfWork uow) : ControllerBase
{
    const string RefreshCookieName = "pl_refresh";
    const string RefreshCsrfHeader = "X-PL-Refresh-CSRF";

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto, CancellationToken ct)
    {
        var result = await auth.RegisterAsync(dto, ct);
        return result.Ok ? Ok(new { message = "registered" }) : BadRequest(result.Errors);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto, CancellationToken ct)
    {
        var reqInfo = new AuthRequestInfo(
            Ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
            UserAgent: Request.Headers.UserAgent.ToString()
        );

        var r = await auth.LoginAsync(dto, reqInfo, ct); 
        if (!r.Ok) return Unauthorized();

        SetRefreshCookie(r.RefreshTokenCookieValue, r.RefreshTokenExpiresAt);

        await uow.auditLogger.LogAsync(new AuditLog
        {
            ActionType = ActionType.Login.ToString(),
            EntityName = EntityName.User.ToString(),
            UserId = r.UserId,
            UserName = r.UserName,
            EntityId = r.UserId.ToString(),
            Route = HttpContext.Request.Path,
            HttpMethod = HttpContext.Request.Method,
            Success = true
        }, ct);

        return Ok(new { accessToken = r.AccessToken, user = r.User });
    }
  
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    {
        // CSRF guard: require same-site JS to send a header
        if (!Request.Headers.ContainsKey(RefreshCsrfHeader))
            return Unauthorized();
       
        var cookie = Request.Cookies[RefreshCookieName];
        if (string.IsNullOrEmpty(cookie)) return Unauthorized();

        var reqInfo = new AuthRequestInfo(
            Ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
            UserAgent: Request.Headers.UserAgent.ToString()
        );

        var r = await auth.RefreshAsync(cookie, reqInfo, ct);
        if (!r.Ok) return Unauthorized();

        SetRefreshCookie(r.RefreshTokenCookieValue, r.RefreshTokenExpiresAt);
        return Ok(new { accessToken = r.AccessToken });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var cookie = Request.Cookies[RefreshCookieName];
        if (!string.IsNullOrEmpty(cookie))
            await auth.LogoutAsync(cookie, ct); // revoke on server
        Response.Cookies.Delete(RefreshCookieName);

        await uow.auditLogger.LogAsync(new AuditLog
        {
            ActionType = "Logout",
            EntityName = "User",
            Route = HttpContext.Request.Path,
            HttpMethod = HttpContext.Request.Method,
            Success = true
        }, ct);

        return Ok(new { message = "logged out" });
    }

    void SetRefreshCookie(string token, DateTime expiresAtUtc)
    {
        Response.Cookies.Append(RefreshCookieName, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = expiresAtUtc,
            IsEssential = true,
        });
    }
}
