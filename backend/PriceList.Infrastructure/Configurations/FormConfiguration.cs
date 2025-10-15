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
    internal class FormConfiguration : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> b)
        {
            b.ToTable("Forms");
            b.HasKey(x => x.Id);

            b.Property(x => x.FormTitle)
                .HasMaxLength(500);

            b.Property(x => x.Rows)
                .IsRequired();

            b.Property(x => x.MinCols)
                .HasDefaultValue(5)
                .IsRequired();

            b.Property(x => x.MaxCols)
                .HasDefaultValue(8)
                .IsRequired();

            b.HasMany(x => x.Columns)
                .WithOne(x => x.Form)
                .HasForeignKey(x => x.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            // Useful index when listing forms
            b.HasIndex(f => new { f.SupplierId, f.BrandId, f.CategoryId, f.ProductGroupId, f.ProductTypeId })
                .IsUnique();
        }
    }
}
