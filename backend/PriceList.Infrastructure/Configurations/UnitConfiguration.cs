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
    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> b)
        {
            b.ToTable("Units");
            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
             .IsRequired()
             .HasMaxLength(200);

            b.HasIndex(x => x.Name)
             .IsUnique();
        }
    }
}
