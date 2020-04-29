using ChallengePad.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengePad.Database
{
    public class EnoDatabaseContextFactory : IDesignTimeDbContextFactory<ChallengePadDbContext>
    {
        public ChallengePadDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChallengePadDbContext>();
            optionsBuilder.UseNpgsql(".");
            return new ChallengePadDbContext(optionsBuilder.Options);
        }
    }

    public class ChallengePadDbContext : DbContext
    {
#pragma warning disable CS8618
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<UploadedFile> Files { get; set; }

        public ChallengePadDbContext(DbContextOptions<ChallengePadDbContext> options) : base(options) { }
#pragma warning restore CS8618

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Operation>()
                .Ignore(b => b.Categories);

            modelBuilder.Entity<Objective>()
                .HasIndex(p => new { p.OperationId, p.Name }).IsUnique();
        }
    }
}
