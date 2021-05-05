using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Data
{
    public class SpaceParkDbContext : DbContext
    {
        public SpaceParkDbContext(DbContextOptions<SpaceParkDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Size>(entity =>
                entity.HasCheckConstraint("CK_Only_Range_Between_1_and_4", "[Type] >= 1 AND [Type] < 5"));
        }

        public DbSet<Parking> Parkings { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<SpacePort> SpacePorts { get; set; }
    }
}
