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
    public class FormCellFeatureValueConfig : IEntityTypeConfiguration<FormCellFeatureValue>
    {
        public void Configure(EntityTypeBuilder<FormCellFeatureValue> b)
        {
            b.ToTable("FormCellFeatureValues");
            b.HasKey(x => x.Id);

            b.Property(x => x.FeatureId).IsRequired();

            // Avoid duplicated feature value per cell + feature
            b.HasIndex(x => new { x.CellId, x.FeatureId }).IsUnique();
        }
    }
}
