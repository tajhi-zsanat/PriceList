using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> b)
        {
            b.ToTable("Brands");
            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Brand ↔ Product
            b.HasMany(br => br.Products)
             .WithOne(p => p.Brand)
             .HasForeignKey(p => p.BrandId)
             .OnDelete(DeleteBehavior.NoAction);

            // Brand ↔ ProductType (M2M)
            b.HasMany(br => br.ProductTypes)
             .WithMany(pt => pt.Brands)
             .UsingEntity<Dictionary<string, object>>(
                "BrandProductType", // join table name
                right => right
                    .HasOne<ProductType>()
                    .WithMany()
                    .HasForeignKey("ProductTypeId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Brand>()
                    .WithMany()
                    .HasForeignKey("BrandId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("BrandProductTypes");
                    join.HasKey("BrandId", "ProductTypeId");
                    join.HasIndex("ProductTypeId");
                    join.HasIndex("BrandId");
                });
        }
    }
}
