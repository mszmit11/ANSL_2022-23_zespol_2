﻿using BOOKING.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BOOKING
{
    public class DbBooking : IdentityDbContext<UserModel>
    {
        public DbBooking(DbContextOptions<DbBooking> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}