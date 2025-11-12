using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Auth;

public record LoginResult(bool Ok, 
    string AccessToken, 
    object User, 
    string RefreshTokenCookieValue, 
    DateTime RefreshTokenExpiresAt);
