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
    internal class FormConfiguration : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> b)
        {
            b.ToTable("Forms");
            b.HasKey(x => x.Id);

            b.Property(x => x.FormTitle)
             .HasMaxLength(200);

            b.HasOne(c => c.Category)
             .WithMany(p => p.Forms)
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(c => c.ProductGroup)
            .WithMany(p => p.Forms)
            .HasForeignKey(p => p.ProductGroupId)
            .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(c => c.ProductType)
            .WithMany(p => p.Forms)
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(c => c.Brand)
            .WithMany(p => p.Forms)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(c => c.Supplier)
            .WithMany(p => p.Forms)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(c => c.Products)
            .WithOne(p => p.Form)
            .HasForeignKey(p => p.FormId)
            .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => new { x.CategoryId, x.ProductGroupId, x.ProductTypeId, x.BrandId, x.SupplierId });

            b.Property(x => x.DisplayOrder).HasDefaultValue(0);
            b.Property(x => x.RowCount).HasDefaultValue(0);
            b.Property(x => x.ColumnCount).HasDefaultValue(0);
        }
    }
}
