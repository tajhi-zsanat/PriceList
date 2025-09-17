using Microsoft.AspNetCore.Mvc;
using PriceList.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PriceList.Api.Dtos
{

    public record ProductListItemDto(
        int Id,
        string? Model,
        string? Description,
        string? DocumentPath,
        long Price,
        int Number,
        IReadOnlyList<string> ProductImages

        //IReadOnlyList<ProductCustomPropertyItemDto> CustomProperties
        //IReadOnlyList<productHeadersDto> productHeaders
    );

    public record productHeadersDto(
        int id,
        string key
    );

    public sealed class ProductCreateForm
    {
        public string Model { get; set; } = default!;

        public string? Description { get; set; }

        public string? DocumentPath { get; set; }

        public long Price { get; set; }

        public int Number { get; set; }

        public int CategoryId { get; set; }

        public int ProductGroupId { get; set; }

        public int ProductTypeId { get; set; }

        public int BrandId { get; set; }

        public int SupplierId { get; set; }

        public int UnitId { get; set; }

        public List<IFormFile>? Image { get; set; }
    }

    public record ProductDetailDto(int Id, string Name, string? Model, string? Description, long UnitPrice, DateTime CreatedAt);
    public record ProductCreateDto(string Name, string? Model, string? Description, long UnitPrice);
    public record ProductUpdateDto(string Name, string? Model, string? Description, long UnitPrice);

    public record ProductCustomPropertyItemDto(string Key, string Value);

    public sealed class ProductCustomPropertyInputDto
    {
        public string Key { get; set; } = default!;
        public string? Value { get; set; }
    }
}
