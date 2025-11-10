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
    public class ProductGroupConfiguration : IEntityTypeConfiguration<ProductGroup>
    {
        public void Configure(EntityTypeBuilder<ProductGroup> b)
        {
            b.ToTable("ProductGroups");

            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Uniqueness per Category
            b.HasIndex(x => new { x.CategoryId, x.Name })
             .IsUnique()
             .HasDatabaseName("UX_ProductGroups_CategoryId_Name");

            // DisplayOrder
            b.Property(x => x.DisplayOrder)
             .HasDefaultValue(0);

            b.HasMany(pg => pg.ProductTypes)
             .WithOne(pt => pt.ProductGroup)       
             .HasForeignKey(pt => pt.ProductGroupId) 
             .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(br => br.Forms)
             .WithOne(p => p.ProductGroup)
             .HasForeignKey(p => p.ProductGroupId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
