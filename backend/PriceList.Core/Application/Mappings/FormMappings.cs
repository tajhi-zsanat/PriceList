using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Mappings
{
    public static class FormMappings
    {
        public static readonly Expression<Func<Form, FormListItemDto>> ToListItem =
            f => new FormListItemDto(f.Id,
                f.FormTitle,
                f.Products.Count,
                f.Category.Name,
                f.ProductGroup.Name,
                f.ProductType.Name,
                f.Brand.Name,
                f.UpdateDate);
    }
}
