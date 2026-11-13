using CampusRooms.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusRooms.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Building> Buildings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Building>().HasData(
                new Building { Id = 1, Name = "Building 1 Name" },
                new Building { Id = 2, Name = "Building 2 Name" },
                new Building { Id = 3, Name = "Building 3 Name" },
                new Building { Id = 4, Name = "Building 4 Name" }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, Code = "MB-101", Name = "Lecture Theatre A", Seats = 120, HasProjector = true, BuildingId = 1 },
                new Room { Id = 2, Code = "MB-204", Name = "Seminar Room", Seats = 30, HasProjector = false, BuildingId = 1 },
                new Room { Id = 3, Code = "SB-110", Name = "Chemistry Lab", Seats = 24, HasProjector = true, BuildingId = 2 },
                new Room { Id = 4, Code = "SB-215", Name = "Computer Lab 2", Seats = 35, HasProjector = true, BuildingId = 2 },
                new Room { Id = 5, Code = "LIB-01", Name = "Study Room 1", Seats = 8, HasProjector = false, BuildingId = 3 }
            );

            modelBuilder.Entity<Room>()
                .HasIndex(room => room.Code)
                .IsUnique();
        }
    }
}