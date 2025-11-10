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

        //public static readonly Expression<Func<Form, FormToIdDto>> ToIdDto =
        //   f => new FormToIdDto(f.Id,
        //       f.BrandId,
        //       f.CategoryId,
        //       f.ProductGroupId,
        //       f.ProductTypeId,
        //       f.SupplierId,
        //       f.RowCount,
        //       f.ColumnCount
        //    );

        // --- Column ---
        //public static readonly Expression<Func<FormColumnDef, FormColumnDto>> ToColumnDtoExpr =
        //    c => new FormColumnDto(
        //        c.Id, c.Index, c.Key, c.Title,
        //        c.Kind, c.Type, c.Required, c.WidthPx, c.FeatureId
        //    );

        //public static FormColumnDto ToColumnDto(FormColumnDef c) => ToColumnDtoExpr.Compile()(c);

        // --- Cell Feature Value ---
        //public static readonly Expression<Func<FormCellFeatureValue, CellFeatureValueDto>> ToCellFeatureDtoExpr =
        //    fv => new CellFeatureValueDto(
        //        fv.FeatureId, fv.Kind,
        //        fv.TextValue, fv.BoolValue, fv.OptionKey, fv.ColorHex
        //    );

        // --- Cell ---
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

        //// --- Form Meta (Id, Title, Rows + Columns) ---
        //public static FormMetaDto ToFormMetaDto(Form f, IEnumerable<FormColumnDef> columns) =>
        //    new FormMetaDto(
        //        f.Id,
        //        f.FormTitle,
        //        f.Rows,
        //        columns
        //            .AsQueryable()
        //            .OrderBy(c => c.Index)
        //            .Select(ToColumnDtoExpr)
        //            .ToList()
        //    );
    }
}
