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
    internal class FormRowConfiguration : IEntityTypeConfiguration<FormRow>
    {
        public void Configure(EntityTypeBuilder<FormRow> b)
        {
            b.ToTable("FormRows");
            b.HasKey(x => x.Id);

            b.HasIndex(x => new { x.FormId, x.RowIndex, x.FormFeatureId })
                .IsUnique();

            b.HasOne(x => x.Form)
              .WithMany(f => f.FormRows)
              .HasForeignKey(x => x.FormId)
              .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.FormFeature)
              .WithMany(f => f.Rows)
              .HasForeignKey(x => x.FormFeatureId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}