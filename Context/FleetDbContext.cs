using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IdSrvr4Demo.Context
{
    public partial class FleetDbContext : DbContext
    {
        public FleetDbContext()
        {
        }

        public FleetDbContext(DbContextOptions<FleetDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Vehicle> Vehicle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=DVTL597LKC2;Initial Catalog=FleetDB;Integrated Security=True;");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(e => e.VehicleId).ValueGeneratedNever();

                entity.Property(e => e.Make).HasMaxLength(100);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.Registration).HasMaxLength(50);
            });
        }
    }
}
