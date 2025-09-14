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
    public class ProductTypeFeatureConfiguration : IEntityTypeConfiguration<ProductTypeFeature> // <- fixed name
    {
        public void Configure(EntityTypeBuilder<ProductTypeFeature> b)
        {
            b.ToTable("ProductTypeFeatures"); // plural is conventional (optional)
            b.HasKey(x => new { x.ProductTypeId, x.ProductFeatureId });

            b.HasOne(x => x.ProductType)
             .WithMany(t => t.ProductTypeFeatures) // <- must match property name
             .HasForeignKey(x => x.ProductTypeId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.ProductFeature)
             .WithMany(f => f.ProductTypeFeatures) // <- must match property name
             .HasForeignKey(x => x.ProductFeatureId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
