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
    public class ProductProductFeatureConfiguration : IEntityTypeConfiguration<ProductProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductProductFeature> b)
        {
            b.ToTable("ProductProductFeatures"); 
            b.HasKey(x => new { x.ProductId, x.ProductFeatureId });

            // Relationships
            b.HasOne(x => x.Product)
                .WithMany(ppf => ppf.ProductFeatures)   
                .HasForeignKey(ppf => ppf.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(ppf => ppf.ProductFeature)
                .WithMany(ppf => ppf.ProductProducts) 
                .HasForeignKey(ppf => ppf.ProductFeatureId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
