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
    public class FormCellConfig : IEntityTypeConfiguration<FormCell>
    {
        public void Configure(EntityTypeBuilder<FormCell> b)
        {
            b.ToTable("FormCells");
            b.HasKey(x => x.Id);

            b.Property(x => x.ColIndex).IsRequired();

            b.Property(x => x.Value).HasMaxLength(250);

            b.HasIndex(c => new { c.RowId, c.ColIndex })
                   .IsUnique();
        }
    }
}