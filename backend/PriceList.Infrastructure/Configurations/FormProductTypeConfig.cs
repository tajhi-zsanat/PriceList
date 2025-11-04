using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Configurations
{
    internal class FormProductTypeConfig : IEntityTypeConfiguration<FormProductType>
    {
        public void Configure(EntityTypeBuilder<FormProductType> b)
        {
            b.ToTable("FormProductTypes");

            b.HasKey(x => new { x.FormId, x.ProductTypeId });

            b.HasOne(x => x.Form)
             .WithMany(f => f.FormProductTypes)
             .HasForeignKey(x => x.FormId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.ProductType)
             .WithMany(f => f.FormProductTypes)
             .HasForeignKey(x => x.ProductTypeId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
