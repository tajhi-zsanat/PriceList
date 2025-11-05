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
        }
    }
}
