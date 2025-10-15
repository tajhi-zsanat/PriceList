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
    internal class FormRowFeatureConfiguration : IEntityTypeConfiguration<FormRowFeature>
    {
        public void Configure(EntityTypeBuilder<FormRowFeature> b)
        {
            b.ToTable("FormRowFeatures");
            b.HasKey(x => x.Id);

            b.HasIndex(x => new { x.RowId, x.FeatureId })
                .IsUnique();
        }
    }
}
