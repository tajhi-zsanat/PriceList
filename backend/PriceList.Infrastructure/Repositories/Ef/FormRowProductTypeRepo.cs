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
    public class FormRowProductTypeRepo : GenericRepository<FormRowProductType>, IFormRowProductTypeRepo
    {

        public FormRowProductTypeRepo(AppDbContext db, ILogger<FormRowProductType> logger)
        : base(db, logger)
        {
        }
    }
}
