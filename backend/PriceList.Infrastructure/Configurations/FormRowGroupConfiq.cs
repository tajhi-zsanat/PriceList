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
    internal class FormRowGroupConfiq : IEntityTypeConfiguration<FormRowProductGroup>
    {
        public void Configure(EntityTypeBuilder<FormRowProductGroup> b)
        {
            b.ToTable("FormRowGroups");

            b.HasKey(x => new { x.FormRowId, x.ProductGroupId });

            b.HasOne(x => x.FormRow)
             .WithMany(r => r.RowProductGroups)
             .HasForeignKey(x => x.FormRowId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.ProductGroup)
             .WithMany(pt => pt.RowProductGroups)
             .HasForeignKey(x => x.ProductGroupId)
             .OnDelete(DeleteBehavior.Restrict);

            b.Property(x => x.FormId).IsRequired();
        }
    }
}
