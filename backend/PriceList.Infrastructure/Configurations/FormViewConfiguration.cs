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
    internal class FormViewConfiguration : IEntityTypeConfiguration<FormView>
    {
        public void Configure(EntityTypeBuilder<FormView> b)
        {
            b.ToTable("FormViews");
            b.HasKey(x => x.Id);

            b.HasIndex(x => new { x.FormId, x.ViewerKey })
                .IsUnique();
        }
    }
}
