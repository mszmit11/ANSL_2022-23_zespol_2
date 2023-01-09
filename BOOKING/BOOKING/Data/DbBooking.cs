﻿using BOOKING.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Reflection.Emit;

namespace BOOKING.Data
{
    public class DbBooking : IdentityDbContext<UserModel>
    {
        public DbBooking(DbContextOptions<DbBooking> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ImageStorageNames> ImageStorageNames { get; set; }
        public DbSet<ImageUrls> ImageUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
    }
}