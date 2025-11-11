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
                f.Category.Name,
                f.ProductGroup.Name,
                f.Rows.ToString(),
                f.Brand.Name,
                f.UpdateDate);

        public static readonly Expression<Func<FormColumnDef, FormColumnDefDto>> ToFormColumnDefDto =
            cell => new FormColumnDefDto(
                cell.FormId,
                cell.Index,
                cell.Kind.ToString(),
                cell.Type.ToString(),
                cell.Key,
                cell.Title
            );

        public static readonly Expression<Func<FormCell, FormCellDto>> ToCellDto =
            cell => new FormCellDto(
                cell.Id,
                cell.Value
            );

        public static readonly Expression<Func<FormCell, FormCellsListItemDto>> ToListCellDto =
            cell => new FormCellsListItemDto(
                cell.Id,
                cell.ColIndex,
                cell.Value
            );
    }
}
