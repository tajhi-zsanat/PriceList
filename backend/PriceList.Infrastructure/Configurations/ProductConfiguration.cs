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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> b)
        {
            b.ToTable("Products");

            b.HasKey(x => x.Id);

            b.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(200);

            //b.Property(x => x.Description)
            //    .HasMaxLength(1000);

            b.Property(x => x.Price)
                .HasColumnType("bigint");

            b.Property(x => x.Number)
                .HasDefaultValue(0);

            // Index for uniqueness across multiple columns
            b.HasIndex(p => new
            {
                p.Model,   
                p.BrandId,
                p.ProductTypeId,
                p.ProductGroupId,
                p.CategoryId,
                p.SupplierId
            })
            .IsUnique();

            //b.Property(x => x.DocumentPath)
            //    .HasMaxLength(500);

            // ----------- RELATIONSHIPS -----------

            // Product ↔ Category (required, but block cascade delete)
            b.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Product ↔ ProductType (optional, no cascade)
            b.HasOne(p => p.ProductType)
                .WithMany(pt => pt.Products)
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Product ↔ Supplier (optional, no cascade)
            b.HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);

            // Product ↔ User (who created product) (optional, no cascade)
            //b.HasOne(p => p.User)
            //    .WithMany(u => u.Products)
            //    .HasForeignKey(p => p.UserId)
            //    .OnDelete(DeleteBehavior.NoAction);

            // Product ↔ Brand (optional, no cascade)
            b.HasOne(p => p.Brand)
                .WithMany(bd => bd.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.NoAction);

            // Product ↔ Unit (optional, no cascade)
            b.HasOne(p => p.Unit)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.NoAction);

            // Product ↔ ProductGroup (optional, no cascade)
            b.HasOne(p => p.ProductGroup)
                .WithMany(pg => pg.Products)
                .HasForeignKey(p => p.ProductGroupId)
                .OnDelete(DeleteBehavior.NoAction);

            // Product → ProductImage (delete product = delete images)
            b.HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
