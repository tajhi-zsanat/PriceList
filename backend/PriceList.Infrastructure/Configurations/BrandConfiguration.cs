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

            // Brand ↔ Form
            b.HasMany(br => br.Forms)
             .WithOne(p => p.Brand)
             .HasForeignKey(p => p.BrandId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
