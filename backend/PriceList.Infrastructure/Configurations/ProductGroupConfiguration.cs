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

            // ProductGroup ↔ Product
            b.HasMany(pg => pg.Products)
             .WithOne(p => p.ProductGroup)
             .HasForeignKey(p => p.ProductGroupId)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
