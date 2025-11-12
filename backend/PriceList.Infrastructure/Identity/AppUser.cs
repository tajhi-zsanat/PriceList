using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PriceList.Infrastructure.Identity
{
    public class AppUser : IdentityUser<int> 
    {
        public string? DisplayName { get; set; }
    }
}
