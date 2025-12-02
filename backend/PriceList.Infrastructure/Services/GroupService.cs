using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Services;
using PriceList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Services
{
    public sealed class GroupService(IUnitOfWork uow) : IGroupService
    {

    }
}
