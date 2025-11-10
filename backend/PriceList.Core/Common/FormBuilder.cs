using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Common
{
    public static class FormBuilder
    {
        public const int MinTotalCols = 8;  // checkbox, rowno, image, file, desc, unit, price, more
        public const int MaxCustomCols = 3;
        public const int MaxTotalCols = MinTotalCols + MaxCustomCols; // 11

        public static List<FormColumnDef> BuildDefaultColumns(int formId, int totalColumns)
        {
            totalColumns = Math.Clamp(totalColumns, MinTotalCols, MaxTotalCols);

            var cols = new List<FormColumnDef>(totalColumns);

            // 0) checkbox (UI only)
            cols.Add(Col(formId, 0, ColumnKind.Static, ColumnType.Checkbox, "checkbox", "انتخاب", required: false));
            // 1) row number (UI only)
            cols.Add(Col(formId, 1, ColumnKind.Static, ColumnType.Rowno, "rowno", "ردیف", required: true));

            // fixed main
            cols.Add(Col(formId, 2, ColumnKind.Fixed, ColumnType.Image, "image", "تصویر"));
            cols.Add(Col(formId, 3, ColumnKind.Fixed, ColumnType.File, "document", "سند"));
            cols.Add(Col(formId, 4, ColumnKind.Fixed, ColumnType.MultilineText, "description", "توضیحات"));
            cols.Add(Col(formId, 5, ColumnKind.Fixed, ColumnType.Select, "unit", "واحد"));

            // compute indexes
            int lastIndex = totalColumns - 1; // "more"
            int priceIndex = lastIndex - 1;
            int firstCustom = 6;
            int customSlots = Math.Max(0, priceIndex - firstCustom);

            for (int i = 0; i < customSlots; i++)
            {
                var type = i switch
                {
                    0 => ColumnType.Custom1,
                    1 => ColumnType.Custom2,
                    2 => ColumnType.Custom3,
                    _ => throw new InvalidOperationException("Exceeded max custom columns.")
                };
                cols.Add(Col(formId, firstCustom + i, ColumnKind.Dynamic, type, $"custom{i + 1}", $"سرگروه {i + 1}"));
            }

            // price
            cols.Add(Col(formId, priceIndex, ColumnKind.Fixed, ColumnType.Price, "price", "قیمت(ریال)", required: true));
            // more (UI only)
            cols.Add(Col(formId, lastIndex, ColumnKind.Static, ColumnType.More, "more", "عملیات"));

            return cols.OrderBy(c => c.Index).ToList();
        }

        public static List<FormRow> BuildDefaultFormRows(int formId, int rows)
        {
            var FormRows = new List<FormRow>();

            for (int r = 0; r < rows; r++)
            {
                FormRows.Add(new FormRow
                {
                    FormId = formId,
                    RowIndex = r
                });
            }

            return FormRows;
        }

        public static List<FormCell> BuildDefaultCells(int formId, IEnumerable<FormColumnDef> columns, IEnumerable<FormRow> formRows)
        {
            var cols = columns.ToList();
            var cells = new List<FormCell>(formRows.Count() * cols.Count);

            foreach (var row in formRows)
            {
                foreach (var c in cols)
                {
                    // Skip UI-only columns
                    if (c.Type is ColumnType.Rowno or ColumnType.More or ColumnType.Checkbox)
                        continue;

                    cells.Add(new FormCell
                    {
                        RowId = row.Id,
                        ColIndex = c.Index,
                        Value = null
                    });
                }
            }

            return cells;
        }

        private static FormColumnDef Col(int formId, int index, ColumnKind kind, ColumnType type, string key, string title, bool required = false)
            => new()
            {
                FormId = formId,
                Index = index,
                Kind = kind,
                Type = type,
                Key = key,
                Title = title,
                Required = required
            };

        public static ColumnType TakeColumnType(int index)
        {
            return index switch
            {
                6 => ColumnType.Custom1,
                7 => ColumnType.Custom2,
                8 => ColumnType.Custom3,
                _ => ColumnType.NotAssign,
            };
        }

        public static string TitleColumn(int index)
        {
            return index switch
            {
                6 => "سرگروه 1",
                7 => "سرگروه 2",
                8 => "سرگروه 3",
                _ => "-",
            };
        }
    }
}
