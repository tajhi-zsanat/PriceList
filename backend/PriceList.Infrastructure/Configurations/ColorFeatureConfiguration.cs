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
    internal class ColorFeatureConfiguration : IEntityTypeConfiguration<ColorFeature>
    {
        public void Configure(EntityTypeBuilder<ColorFeature> b)
        {
            b.ToTable("ColorFeatures");
        }
    }
}
