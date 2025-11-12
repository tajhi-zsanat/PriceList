using PriceList.Core.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterDto dto, CancellationToken ct);
        Task<LoginResult> LoginAsync(LoginDto dto, AuthRequestInfo req, CancellationToken ct);
        Task<RefreshResult> RefreshAsync(string refreshCookieValue, AuthRequestInfo req, CancellationToken ct);
        Task LogoutAsync(string refreshCookieValue, CancellationToken ct);
    }
}
