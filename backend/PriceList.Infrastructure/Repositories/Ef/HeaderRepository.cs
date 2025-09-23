using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class HeaderRepository : GenericRepository<Header>, IHeaderRepository
    {
        public HeaderRepository(AppDbContext db, ILogger<Header> logger)
        : base(db, logger)
        {
        }
    }
}
