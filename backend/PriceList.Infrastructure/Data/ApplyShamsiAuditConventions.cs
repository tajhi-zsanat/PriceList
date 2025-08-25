using Microsoft.EntityFrameworkCore;
using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Data
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyShamsiAuditConventions(this ModelBuilder modelBuilder)
        {
            foreach (var et in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(ShamsiAuditableEntity).IsAssignableFrom(et.ClrType))
                    continue;

                var eb = modelBuilder.Entity(et.ClrType);

                // Apply only if the property actually exists on the entity
                if (et.FindProperty(nameof(ShamsiAuditableEntity.CreateDate)) != null)
                    eb.Property<string>(nameof(ShamsiAuditableEntity.CreateDate)).HasMaxLength(10).IsUnicode(false);

                if (et.FindProperty(nameof(ShamsiAuditableEntity.UpdateDate)) != null)
                    eb.Property<string>(nameof(ShamsiAuditableEntity.UpdateDate)).HasMaxLength(10).IsUnicode(false);

                if (et.FindProperty(nameof(ShamsiAuditableEntity.CreateTime)) != null)
                    eb.Property<string>(nameof(ShamsiAuditableEntity.CreateTime)).HasMaxLength(4).IsUnicode(false);

                if (et.FindProperty(nameof(ShamsiAuditableEntity.UpdateTime)) != null)
                    eb.Property<string>(nameof(ShamsiAuditableEntity.UpdateTime)).HasMaxLength(4).IsUnicode(false);
            }
        }
    }
}
