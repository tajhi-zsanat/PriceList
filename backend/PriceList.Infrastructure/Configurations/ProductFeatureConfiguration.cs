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
    public class ProductFeatureConfiguration : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> b)
        {
            b.ToTable("ProductFeatures");
            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
             .IsRequired()
             .HasMaxLength(200);

            b.HasIndex(x => x.Name).IsUnique();
        }
    }
}
