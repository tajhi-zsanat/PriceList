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
    internal class ProductCustomPropertyConfiguration : IEntityTypeConfiguration<ProductCustomProperty>
    {
        public void Configure(EntityTypeBuilder<ProductCustomProperty> b)
        {
            b.ToTable("ProductCustomProperties");

            b.HasKey(x => x.Id);

            b.Property(x => x.Value)
                .HasMaxLength(256)
                .IsRequired();

            b.HasOne(x => x.Product)
                .WithMany(p => p.CustomProperties)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.productHeader)
                .WithMany(p => p.CustomProperties)
                .HasForeignKey(x => x.productHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            // unique per product/key
            b.HasIndex(x => new { x.ProductId, x.productHeaderId }).IsUnique();
        }
    }
}
