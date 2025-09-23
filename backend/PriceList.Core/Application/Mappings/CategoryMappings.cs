using PriceList.Core.Application.Dtos.Category;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Mappings
{
    public static class CategoryMappings
    {
        public static readonly Expression<Func<Category, CategoryListItemDto>> ToListItem =
        p => new CategoryListItemDto(p.Id, p.Name, p.ImagePath);
    }
}
