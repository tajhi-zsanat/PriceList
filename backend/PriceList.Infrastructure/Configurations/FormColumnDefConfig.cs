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
    public class FormColumnDefConfig : IEntityTypeConfiguration<FormColumnDef>
    {
        public void Configure(EntityTypeBuilder<FormColumnDef> b)
        {
            b.ToTable("FormColumnDefs");
            b.HasKey(x => x.Id);

            b.Property(x => x.Index).IsRequired();

            b.Property(x => x.Key)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(x => x.Title)
                .HasMaxLength(150)
                .IsRequired();

            b.Property(x => x.Kind)
                .HasConversion<int>() // store enum as int
                .IsRequired();

            b.Property(x => x.Type)
                .HasConversion<int>()
                .IsRequired();

            b.Property(x => x.Required)
                .HasDefaultValue(false)
                .IsRequired();

            b.Property(x => x.WidthPx);

            b.HasIndex(c => new { c.FormId, c.Index })
              .IsUnique();

            b.HasIndex(x => new { x.FormId, x.Key }).IsUnique();   // keys unique per form

            // Optional Feature link (keep as simple FK if you have Feature table)
            // b.HasOne<Feature>().WithMany().HasForeignKey(x => x.FeatureId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
