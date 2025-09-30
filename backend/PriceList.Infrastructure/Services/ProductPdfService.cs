using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Services;
// using PriceList.Core.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using static QuestPDF.Fluent.ElementExtensions;

namespace PriceList.Infrastructure.Services;

public sealed class ProductPdfService : IProductPdfService
{
    private readonly IFeatureRepository _features;
    private readonly byte[]? _hardImage; // loaded once

    // 🔧 Hardcoded path: change this to your real groupfolder image
    private const string HardImagePath = @"E:\Projects\PriceList\backend\PriceList.Api\wwwroot\uploads\product-test.png";
    // If you’re on Linux, use: "wwwroot/uploads/products/groupfolder/sample.jpg"

    public ProductPdfService(IFeatureRepository features)
    {
        _features = features;

        try
        {
            var fullPath = Path.IsPathRooted(HardImagePath)
                ? HardImagePath
                : Path.Combine(AppContext.BaseDirectory, HardImagePath);

            if (File.Exists(fullPath))
                _hardImage = File.ReadAllBytes(fullPath);
        }
        catch
        {
            // ignore; we'll render a placeholder if image not available
        }
    }

    // Column widths (mm) — ordered RTL: [Supplier | Price | Name | Code]
    private const float ColSupp = 50f;
    private const float ColPrice = 50f;
    private const float ColName = 50f;
    private const float ColCode = 50f;
    private const float ColImg = 22f;  // was 18

    public async Task<byte[]> GenerateAsync(int productId, CancellationToken ct = default)
    {
        // TODO: replace placeholders with real filters passed by caller
        var result = await _features.ListFeaturesWithProductsScrollAsync(
            skip: 0, take: 500, categoryId: 1, groupId: 1, typeId: 1, brandId: 2, supplierId: 1, ct: ct);

        var rows = new List<ProductRow>();

        foreach (var bucket in result.Items)
        {
            foreach (var p in bucket.Products)
            {
                ct.ThrowIfCancellationRequested();

                rows.Add(new ProductRow(
                    Code: "—",
                    Name: p.Description ?? "—",
                    Supplier: "—",
                    Price: 0m,
                    FeatureText: BuildFeatureText(bucket) // e.g., "رنگ: قرمز | سایز: M"
                ));
            }
        }

        // If you want exactly 30 rows:
        // rows = rows.Take(30).ToList();

        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);

                page.DefaultTextStyle(x => x.FontFamily("Vazir").FontSize(9));

                page.Header().Element(ComposeHeader);
                page.Content().Element(c => ComposeTable(c, rows, ct));
                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span(FaDigits("صفحه "));
                    t.CurrentPageNumber();
                    t.Span(FaDigits(" از "));
                    t.TotalPages();
                });
            });
        });

        return doc.GeneratePdf();
    }

    // ======= Layout parts =======

    private void ComposeHeader(IContainer container)
    {
        container.Column(col =>
        {
            // Title + Date row
            col.Item().Row(row =>
            {
                row.RelativeItem().AlignRight().Text(t =>
                {
                    t.Span("لیست قیمت محصولات").Bold().FontSize(12);
                    t.Line(FaDigits($"تاریخ: {ToShamsi(DateTime.UtcNow)}")).FontSize(9).FontColor(Colors.Grey.Darken2);
                });
            });

            // Filters “chips” row
            col.Item().PaddingTop(6).Row(r =>
            {
                // push chips to the left to balance the title on the right
                r.RelativeItem();
                r.AutoItem().Element(FilterChip).Text("برند: میراب");
                // add more chips if you like:
                // r.AutoItem().Element(FilterChip).Text("دسته: ...");
                // r.AutoItem().Element(FilterChip).Text("گروه: ...");
                // r.AutoItem().Element(FilterChip).Text("نوع: ...");
            });

            // Divider
            col.Item().PaddingTop(6).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
        });

        static IContainer FilterChip(IContainer c) =>
            c.PaddingVertical(3).PaddingHorizontal(6)
             .Background(Colors.Grey.Lighten4)
             .Border(0.5f).BorderColor(Colors.Grey.Lighten2);
    }

    private void ComposeTable(IContainer container, IReadOnlyList<ProductRow> rows, CancellationToken ct)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(ColPrice); // قیمت
                columns.ConstantColumn(ColImg);   // 🖼️ تصویر
                columns.ConstantColumn(ColSupp);  // تأمین‌کننده
                columns.RelativeColumn(ColName);  // نام/ویژگی
                columns.ConstantColumn(ColCode);  // کد/مدل
            });

            // ------ Header (define ONCE) ------
            table.Header(header =>
            {
                header.Cell().Element(h => HeaderCell(h, narrow: true).AlignCenter())
                      .Text(t => t.Span("تصویر").FontSize(8).SemiBold());

                header.Cell().Element(h => HeaderCell(h).AlignCenter())
                        .Text(t => t.Span("کد/مدل").FontSize(9).SemiBold());

                header.Cell().Element(h => HeaderCell(h).AlignCenter())
                      .Text(t => t.Span("تأمین‌کننده").FontSize(9).SemiBold());

                header.Cell().Element(h => HeaderCell(h).AlignCenter())
                      .Text(t => t.Span("قیمت").FontSize(9).SemiBold());

                header.Cell().Element(h => HeaderCell(h).AlignRight())
                      .Text(t => t.Span("نام / ویژگی‌ها").FontSize(9).SemiBold());
            });

            // ------ Body rows ------
            for (int i = 0; i < rows.Count; i++)
            {
                ct.ThrowIfCancellationRequested();
                var r = rows[i];
                bool zebra = i % 2 == 1;

                // Supplier
                table.Cell().Element(c => BodyCell(c, zebra).AlignCenter())
                    .Text(FaDigits(EmptyDash(r.Supplier))).FontSize(9);

                // Price
                table.Cell().Element(c => BodyCell(c, zebra).AlignCenter())
                    .Text(FaDigits(FormatPrice(r.Price))).FontSize(9);

                // Name + features
                table.Cell().Element(c => BodyCell(c, zebra))
                    .Column(cc =>
                    {
                        cc.Item().AlignRight().Text(EmptyDash(r.Name)).SemiBold().FontSize(9.5f);
                        if (!string.IsNullOrWhiteSpace(r.FeatureText))
                            cc.Item().AlignRight().Text(r.FeatureText)
                               .FontSize(8.5f).FontColor(Colors.Grey.Darken2);
                    });

                // Code / Model
                table.Cell().Element(c => BodyCell(c, zebra).AlignCenter())
                    .Text(FaDigits(EmptyDash(r.Code))).FontSize(9);

                // Image
                table.Cell().Element(c => BodyCell(c, zebra).AlignCenter())
                    .Element(c =>
                    {
                        if (_hardImage is not null)
                        {
                            // One chain, one terminal child (Image)
                            c.MinHeight(16)
                             .AlignMiddle().AlignCenter()
                             .Image(_hardImage).FitArea();
                            return;
                        }

                        // One chain, one terminal child (Text)
                        c.MinHeight(16)
                         .Border(0.8f).BorderColor(Colors.Grey.Lighten2)
                         .Padding(2)
                         .AlignMiddle().AlignCenter()
                         .Text("No Image").FontSize(7).FontColor(Colors.Grey.Darken1);
                    });
            }

            static IContainer BodyCell(IContainer c, bool zebra) =>
                c.PaddingVertical(4).PaddingHorizontal(6)
                 .Background(zebra ? Colors.Grey.Lighten5 : Colors.White)
                 .BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten3);
        });
    }

    // ======= Helpers =======

    // default header cell, plus a narrow version for tight columns
    static IContainer HeaderCell(IContainer c, bool narrow = false) =>
        c.PaddingVertical(5)
         .PaddingHorizontal(narrow ? 3 : 6)   // narrower padding for the image column
         .Background(Colors.Grey.Lighten3)
         .BorderBottom(1).BorderColor(Colors.Grey.Lighten2);

    private static string EmptyDash(string? s) => string.IsNullOrWhiteSpace(s) ? "—" : s!;

    private static string BuildFeatureText(object bucket)
    {
        // TODO: Map your bucket → readable Persian features string.
        // Example: return $"رنگ: {FaDigits(color)} | سایز: {FaDigits(size)}";
        return "";
    }

    private static string ToShamsi(DateTime utc)
    {
        var dt = utc.ToLocalTime();
        var pc = new PersianCalendar();
        var y = pc.GetYear(dt);
        var m = pc.GetMonth(dt);
        var d = pc.GetDayOfMonth(dt);
        var time = dt.ToString("HH:mm"); // keep latin time or convert digits below
        return $"{y:0000}/{m:00}/{d:00} {time}";
    }

    private static string FormatPrice(decimal price)
    {
        if (price <= 0) return "—";
        // Format with thousand separators, then add currency text
        var s = string.Format(CultureInfo.InvariantCulture, "{0:N0}", price);
        return $"{s} تومان";
    }

    private static string FaDigits(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        // 0..9 -> ۰..۹
        return input
            .Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴")
            .Replace("5", "۵").Replace("6", "۶").Replace("7", "۷").Replace("8", "۸").Replace("9", "۹");
    }

    private sealed record ProductRow(
        string Code,
        string Name,
        string Supplier,
        decimal Price,
        string FeatureText
    );
}
