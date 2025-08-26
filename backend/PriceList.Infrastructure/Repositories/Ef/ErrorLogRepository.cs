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
    public class ErrorLogRepository(AppDbContext db) : GenericRepository<ErrorLog>(db), IErrorLogRepository
    {
    }
}
