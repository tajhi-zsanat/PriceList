using Microsoft.EntityFrameworkCore;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<Product> Products => Set<Product>();
        //public DbSet<ProductHeader> ProductHeader => Set<ProductHeader>();
        //public DbSet<Header> Headers => Set<Header>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
        public DbSet<ProductType> ProductTypes => Set<ProductType>();
        //public DbSet<Feature> Features => Set<Feature>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        //public DbSet<ProductTypeFeature> ProductTypeFeature => Set<ProductTypeFeature>();
        //public DbSet<ProductFeature> ProductFeatures => Set<ProductFeature>();
        public DbSet<Unit> Units => Set<Unit>();
        //public DbSet<ColorFeature> ColorFeatures => Set<ColorFeature>();
        public DbSet<ErrorLog> ErrorLog => Set<ErrorLog>();

        public DbSet<Form> Forms => Set<Form>();
        public DbSet<FormColumnDef> FormColumnDefs => Set<FormColumnDef>();
        public DbSet<FormCell> FormCells => Set<FormCell>();
        //public DbSet<FormCellFeatureValue> FormCellFeatureValues => Set<FormCellFeatureValue>();
        public DbSet<FormRow> FormRows => Set<FormRow>();
        public DbSet<FormProductGroup> FormProductGroups => Set<FormProductGroup>();
        public DbSet<FormRowProductGroup> FormRowProductGroups => Set<FormRowProductGroup>();

        //public DbSet<FormRowFeature> FormRowFeatures => Set<FormRowFeature>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.ApplyShamsiAuditConventions();
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var utcNow = DateTime.UtcNow;

            // Iran timezone + Persian calendar helpers (inline for brevity)
            var iranTz = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time")
                : TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");
            var local = TimeZoneInfo.ConvertTimeFromUtc(utcNow, iranTz);
            var pc = new PersianCalendar();
            string persianDate = $"{pc.GetYear(local):0000}/{pc.GetMonth(local):00}/{pc.GetDayOfMonth(local):00}";
            string hhmm = local.ToString("HHmm");

            foreach (var e in ChangeTracker.Entries<ShamsiAuditableEntity>())
            {
                if (e.State == EntityState.Added)
                {
                    e.Entity.CreateDateAndTime = utcNow;
                    e.Entity.UpdateDateAndTime = utcNow;

                    e.Entity.CreateDate = persianDate;
                    e.Entity.CreateTime = hhmm;
                    e.Entity.UpdateDate = persianDate;
                    e.Entity.UpdateTime = hhmm;
                }
                else if (e.State == EntityState.Modified)
                {
                    e.Entity.UpdateDateAndTime = utcNow;
                    e.Entity.UpdateDate = persianDate;
                    e.Entity.UpdateTime = hhmm;
                }
            }

            return base.SaveChangesAsync(ct);
        }
    }
}
