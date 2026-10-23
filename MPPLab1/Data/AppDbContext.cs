using Microsoft.EntityFrameworkCore;
using MPPLab1.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MPPLab1.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(t => t.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    public class SqliteDbContext : AppDbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }
    }

    public class PostgresDbContext : AppDbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) { }
    }
}