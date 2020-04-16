using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Zone> Zones { get; set; }

        public override int SaveChanges()
        {
            DoThingsBeforeSaveChange();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DoThingsBeforeSaveChange();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void DoThingsBeforeSaveChange()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseModel && (e.State == EntityState.Modified || e.State == EntityState.Added));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
                else
                {
                    ((BaseModel)entityEntry.Entity).ModifiedAt = DateTime.Now;
                }
            }
        }
    }
}
