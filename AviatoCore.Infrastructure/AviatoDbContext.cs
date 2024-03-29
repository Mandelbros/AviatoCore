﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AviatoCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AviatoCore.Infrastructure
{
    public class AviatoDbContext : IdentityDbContext<User>
    {
        public AviatoDbContext(DbContextOptions<AviatoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientType> ClientTypes { get; set; }
        public DbSet<Plane> Planes { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<FacilityType> FacilityTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ClientService> ClientServices { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<OwnerRole> OwnersRole { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<RepairType> RepairTypes { get; set; }
        public DbSet<FlightRepair> FlightRepairs { get; set; }
        public DbSet<FlightService> FlightServices { get; set; }
        public DbSet<RepairDependency> RepairDependencies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PlaneCondition> PlaneConditions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Don't forget to call base method
            
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => x.UserId);

            modelBuilder.Entity<RepairDependency>()
           .HasKey(r => new { r.PlaneConditionId, r.RepairAId, r.RepairBId });

            modelBuilder.Entity<FlightService>()
            .HasOne(fs => fs.Flight)
            .WithMany(f => f.FlightServices)
            .HasForeignKey(fs => fs.FlightId)
            .OnDelete(DeleteBehavior.NoAction); // Add this line

            modelBuilder.Entity<FlightService>()
                .HasOne(fs => fs.Service)
                .WithMany(s => s.FlightServices)
                .HasForeignKey(fs => fs.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FlightRepair>()
                .HasOne(fr => fr.Repair)
                .WithMany(r => r.FlightRepairs)
                .HasForeignKey(fr => fr.RepairId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FlightRepair>()
                .HasOne(fr => fr.Flight)
                .WithMany(f => f.FlightRepairs)
                .HasForeignKey(fr => fr.FlightId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RepairDependency>()
                .HasOne(rd => rd.RepairA)
                .WithMany(r => r.RepairADependencies)
                .HasForeignKey(rd => rd.RepairAId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RepairDependency>()
                .HasOne(rd => rd.RepairB)
                .WithMany(r => r.RepairBDependencies)
                .HasForeignKey(rd => rd.RepairBId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Airport>().HasData(
                new Airport { Id = 1, Name = "José Martí", Address = "Avenida Van Troy y Final, Rancho Boyeros, Havana, Cuba", Latitude = 22.9892, Longitude = -82.4092 },
                new Airport { Id = 2, Name = "Juan Gualberto Gómez", Address = "Matanzas, Cuba", Latitude = 23.0344, Longitude = -81.4353 },
                new Airport { Id = 3, Name = "Abel Santamaría", Address = "Carretera a Maleza Km 1 y medio, Santa Clara, Cuba", Latitude = 22.4922, Longitude = -79.9436 },
                new Airport { Id = 4, Name = "Frank País", Address = "Holguín, Cuba", Latitude = 20.7856, Longitude = -76.3151 },
                new Airport { Id = 5, Name = "Playa Baracoa", Address = "Playa Baracoa, Havana, Cuba", Latitude = 23.0328, Longitude = -82.5794 }
            );

            // Add seed data for Role
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Director", NormalizedName = "DIRECTOR" },
                new IdentityRole { Id = "3", Name = "Security", NormalizedName = "SECURITY" },
                new IdentityRole { Id = "4", Name = "Maintenance", NormalizedName = "MAINTENANCE" },
                new IdentityRole { Id = "5", Name = "Client", NormalizedName = "CLIENT" }
            );

            modelBuilder.Entity<ClientType>().HasData(
               new ClientType { Id = 1, Name = "Regular" },
               new ClientType { Id = 2, Name = "VIP"},
               new ClientType { Id = 3, Name = "Company"}
            );

            modelBuilder.Entity<FacilityType>().HasData(
                    new FacilityType { Id = 1, Name = "Cafeteria" },
                    new FacilityType { Id = 2, Name = "Workshop" },
                    new FacilityType { Id = 3, Name = "Clothing Store" },
                    new FacilityType { Id = 4, Name = "Gift Shop" },
                    new FacilityType { Id = 5, Name = "Currency exchange office" },
                    new FacilityType { Id = 6, Name = "Sushi Bar" },
                    new FacilityType { Id = 7, Name = "Restaurant" }
            );
     
            modelBuilder.Entity<Facility>().HasData(
                   new Facility { Id = 1, Name = "Breadway",Address="Street 15, 14077",ImgUrl= "https://res.cloudinary.com/dp9wcmorr/image/upload/v1711663766/womxzvcwlkmgebmkzypa.webp", AirportId=1, FacilityTypeId=1 },
                   new Facility { Id = 2, Name = "AMXWorkshop", Address = "Street 20, 23078", ImgUrl = "https://res.cloudinary.com/dp9wcmorr/image/upload/v1711663767/yej7dkz5v8nwp1cemm5l.jpg", AirportId = 1, FacilityTypeId = 2 },
                   new Facility { Id = 3, Name = "Tascon", Address = "Street 5, 66778", ImgUrl = "https://res.cloudinary.com/dp9wcmorr/image/upload/v1711663768/gt2fpdjqrqoqqrvrltm5.jpg", AirportId = 1, FacilityTypeId = 3 },
                   new Facility { Id = 4, Name = "ArtesaniaDominicana", Address = "Street 1, 45556", ImgUrl = "https://res.cloudinary.com/dp9wcmorr/image/upload/v1711663767/kbxqsrk2vu5xstwxrvxr.jpg", AirportId = 1, FacilityTypeId = 4 },
                   new Facility { Id = 5, Name = "CambioExchange", Address = "Street 20, 23078", ImgUrl = "https://res.cloudinary.com/dp9wcmorr/image/upload/v1711663767/i4tka668odgaukhbiigd.jpg", AirportId = 1, FacilityTypeId = 5},
                   new Facility { Id = 6, Name = "Ryu", Address = "Street 7, 12078", ImgUrl = "https://res.cloudinary.com/dp9wcmorr/image/upload/v1711663766/ryd91lefb0jsz0sfgr8x.jpg", AirportId = 2, FacilityTypeId = 6 },
                   new Facility { Id = 7, Name = "Tagliatella", Address = "Street 1, 16078", ImgUrl = "https://res.cloudinary.com/dp9wcmorr/image/upload/v1711663766/ovqroknpskzubu6g3trd.jpg", AirportId = 2, FacilityTypeId = 7 }
           );

            modelBuilder.Entity<Plane>().HasData(
                new Plane
                {
                    Id = 1,
                    Classification = "Commercial",
                    CargoCapacity = 20000,
                    CrewCount = 5,
                    PassengerCapacity = 200,
                    OwnerId = "684c656f-0424-4c06-9a2e-92bac4f3d9bd"
                },
                new Plane
                {
                    Id = 2,
                    Classification = "Private",
                    CargoCapacity = 5000,
                    CrewCount = 2,
                    PassengerCapacity = 10,
                    OwnerId = "8e03cd57-c768-4a44-b174-45a450441b44"
                },
                new Plane
                {
                    Id = 3,
                    Classification = "Cargo",
                    CargoCapacity = 50000,
                    CrewCount = 5,
                    PassengerCapacity = 0,
                    OwnerId = "029eaca6-cb0f-408b-b6c4-c51cea6e5441"
                },
                new Plane
                {
                    Id = 4,
                    Classification = "Military",
                    CargoCapacity = 15000,
                    CrewCount = 10,
                    PassengerCapacity = 50,
                    OwnerId = "029eaca6-cb0f-408b-b6c4-c51cea6e5441"
                },
                new Plane
                {
                    Id = 5,
                    Classification = "Commercial",
                    CargoCapacity = 25000,
                    CrewCount = 6,
                    PassengerCapacity = 250,
                    OwnerId = "684c656f-0424-4c06-9a2e-92bac4f3d9bd"
                }
            );


        }
    }
}
