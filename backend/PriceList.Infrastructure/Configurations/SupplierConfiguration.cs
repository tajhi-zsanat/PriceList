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
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> b)
        {
            b.ToTable("Suppliers");
            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
             .IsRequired()
             .HasMaxLength(200);

            b.HasIndex(x => x.Name)
             .IsUnique();

            b.Property(x => x.DisplayOrder)
             .HasDefaultValue(0);

            b.HasMany(c => c.Products)
             .WithOne(p => p.Supplier)
             .HasForeignKey(p => p.SupplierId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
