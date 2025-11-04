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
    internal class FormRowProductTypeConfiq : IEntityTypeConfiguration<FormRowProductType>
    {
        public void Configure(EntityTypeBuilder<FormRowProductType> b)
        {
            b.ToTable("FormRowProductTypes");

            b.HasKey(x => new { x.FormRowId, x.ProductTypeId });

            b.HasOne(x => x.FormRow)
             .WithMany(r => r.RowProductTypes)
             .HasForeignKey(x => x.FormRowId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.ProductType)
             .WithMany(pt => pt.RowProductTypes)
             .HasForeignKey(x => x.ProductTypeId)
             .OnDelete(DeleteBehavior.Restrict);

            b.Property(x => x.FormId).IsRequired();
        }
    }
}
