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

        public DbSet<Parking> Parkings { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<SpacePort> SpacePorts { get; set; }
    }
}
