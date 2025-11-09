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
    internal class FormFeatureConfiq : IEntityTypeConfiguration<FormFeature>
    {
        public void Configure(EntityTypeBuilder<FormFeature> b)
        {
            b.ToTable("FormFeatures");

            b.HasKey(x => x.Id);

            b.Property(x => x.Color)
              .HasMaxLength(32);

            // Indexes / uniqueness
            b.HasIndex(x => new { x.FormId, x.DisplayOrder, x.Name })
                .IsUnique();

            // Relationship
            b.HasOne(x => x.Form)
                .WithMany(f => f.FormFeatures)
                .HasForeignKey(x => x.FormId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
