using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Identity;
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
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
        public DbSet<ProductType> ProductTypes => Set<ProductType>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Unit> Units => Set<Unit>();
        public DbSet<ErrorLog> ErrorLog => Set<ErrorLog>();

        public DbSet<Form> Forms => Set<Form>();
        public DbSet<FormColumnDef> FormColumnDefs => Set<FormColumnDef>();
        public DbSet<FormCell> FormCells => Set<FormCell>();
        public DbSet<FormRow> FormRows => Set<FormRow>();
        public DbSet<FormFeature> FormFeature => Set<FormFeature>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.ApplyShamsiAuditConventions();
            base.OnModelCreating(modelBuilder);

            // GLOBAL SOFT DELETE FILTER
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext).GetMethod(nameof(ApplySoftDeleteFilter),
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }

            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.HasIndex(x => x.Token).IsUnique();
                e.Property(x => x.Token).IsRequired().HasMaxLength(256);
                e.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private static void ApplySoftDeleteFilter<TEntity>(ModelBuilder builder)
        where TEntity : class, ISoftDelete
        {
            builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
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

            // --- Soft delete ---
            foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.IsDeleted = false;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    // convert hard delete to soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                }
            }

            return base.SaveChangesAsync(ct);
        }
    }
}
