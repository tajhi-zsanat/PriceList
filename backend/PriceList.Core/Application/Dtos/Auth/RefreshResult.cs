using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Auth;

public record RefreshResult(bool Ok, 
    string AccessToken, 
    string RefreshTokenCookieValue, 
    DateTime RefreshTokenExpiresAt);
