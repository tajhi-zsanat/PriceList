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
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> b)
        {
            b.ToTable("ProductTypes");
            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            // ProductType ↔ Product
            b.HasMany(pt => pt.Products)
             .WithOne(p => p.ProductType)
             .HasForeignKey(p => p.ProductTypeId)
             .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(pt => pt.ProductGroup)
             .WithMany(pg => pg.ProductTypes)
             .HasForeignKey(pt => pt.ProductGroupId)
             .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
