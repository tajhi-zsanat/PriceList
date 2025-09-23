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

            b.HasKey(pf => new { pf.ProductId, pf.FeatureId });

            b.HasOne(pf => pf.Product)
            .WithMany(p => p.ProductFeatures)
            .HasForeignKey(pf => pf.ProductId);

            b.HasOne(pf => pf.Feature)
            .WithMany(p => p.ProductFeatures)
            .HasForeignKey(pf => pf.FeatureId);
        }
    }
}
