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
    internal class productHeaderConfiguration : IEntityTypeConfiguration<productHeader>
    {
        public void Configure(EntityTypeBuilder<productHeader> b)
        {
            b.ToTable("productHeaders");

            b.HasKey(x => x.Id);
            b.Property(p => p.Id)
                .IsRequired();

            b.HasIndex(p => new { p.BrandId, p.ProductTypeId })
               .IsUnique();

            // Relationships
            b.HasOne(x => x.Brand)
                .WithMany(t => t.productHeaders)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.ProductType)
                .WithMany(t => t.productHeaders)
                .HasForeignKey(x => x.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
