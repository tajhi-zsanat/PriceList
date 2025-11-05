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
    internal class FormProductGroupConfig : IEntityTypeConfiguration<FormProductGroup>
    {
        public void Configure(EntityTypeBuilder<FormProductGroup> b)
        {
            b.ToTable("FormProductGroups");

            b.HasKey(x => new { x.FormId, x.ProductGroupId });

            b.HasOne(x => x.Form)
             .WithMany(f => f.FormProductGroups)
             .HasForeignKey(x => x.FormId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.ProductGroup)
             .WithMany(f => f.FormProductGroups)
             .HasForeignKey(x => x.ProductGroupId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
