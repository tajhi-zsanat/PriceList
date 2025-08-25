using Microsoft.EntityFrameworkCore;
using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyShamsiAuditConventions(this ModelBuilder modelBuilder)
        {
            var types = modelBuilder.Model.GetEntityTypes()
                .Where(t => typeof(ShamsiAuditableEntity).IsAssignableFrom(t.ClrType))
                .Select(t => t.ClrType)
                .Distinct();

            foreach (var t in types)
            {
                modelBuilder.Entity(t).Property<string?>(nameof(ShamsiAuditableEntity.CreateDate)).HasMaxLength(10);
                modelBuilder.Entity(t).Property<string?>(nameof(ShamsiAuditableEntity.CreateTime)).HasMaxLength(4);
                modelBuilder.Entity(t).Property<string?>(nameof(ShamsiAuditableEntity.UpdateDate)).HasMaxLength(10);
                modelBuilder.Entity(t).Property<string?>(nameof(ShamsiAuditableEntity.UpdateTime)).HasMaxLength(4);
            }
        }
    }
}
