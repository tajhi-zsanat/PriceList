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
    public class FormRowIRepository : GenericRepository<FormRow>, IFormRowRepository
    {
        public FormRowIRepository(AppDbContext db, ILogger<FormRow> logger)
       : base(db, logger)
        {
        }
    }
}
