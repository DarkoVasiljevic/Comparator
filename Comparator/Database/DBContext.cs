using Comparator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Database
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options) {}

        public virtual DbSet<Left> Lefts { get; set; }
        public virtual DbSet<Right> Rights { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<Diff> Diffs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Left>()
                .HasMany(m => m.Results)
                .WithOne(o => o.Left)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Left>()
                .Property(l => l.ModifiedDate)
                .HasDefaultValueSql("getdate()");

            builder
                .Entity<Right>()
                .HasMany(m => m.Results)
                .WithOne(o => o.Right)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Right>()
                .Property(r => r.ModifiedDate)
                .HasDefaultValueSql("getdate()");

            builder
                .Entity<Result>()
                .HasOne(o => o.Left)
                .WithMany(m => m.Results)
                .HasForeignKey(fk => fk.LeftId);

            builder
                .Entity<Result>()
                .HasOne(o => o.Right)
                .WithMany(m => m.Results)
                .HasForeignKey(fk => fk.RightId);

            builder
                .Entity<Result>()
                .Property(r => r.ComparationDate)
                .HasDefaultValueSql("getdate()");

            builder
                .Entity<Result>()
                .HasMany(m => m.Diffs)
                .WithOne(o => o.Result)
                .HasForeignKey(fk => fk.ResultId);

            builder
               .Entity<Diff>()
               .HasOne(o => o.Result)
               .WithMany(m => m.Diffs)
               .HasForeignKey(fk => fk.ResultId);
        }

        public override int SaveChanges()
        {
            var leftRecords = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Left && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .ToList();

            var rightRecords = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Right && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .ToList();

            var resultRecords = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Result && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .ToList();

            foreach (var entity in leftRecords)
                (entity.Entity as Left).ModifiedDate = DateTime.Now;

            foreach (var entity in rightRecords)
                (entity.Entity as Right).ModifiedDate = DateTime.Now;

            foreach (var entity in resultRecords)
                (entity.Entity as Result).ComparationDate = DateTime.Now;

            return base.SaveChanges();
        }
    }
}
