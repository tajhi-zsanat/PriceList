using PriceList.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Auth
{
    public interface ITokenService
    {
        string CreateAccessToken(AppUser user, IList<string> roles, IList<System.Security.Claims.Claim> userClaims);
        (string token, DateTime expiresAt) CreateRefreshToken();
    }
}
