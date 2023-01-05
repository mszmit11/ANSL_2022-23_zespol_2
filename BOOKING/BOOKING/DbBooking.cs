using BOOKING.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Reflection.Emit;

namespace BOOKING
{
    public class DbBooking : IdentityDbContext<UserModel>
    {
        public DbBooking(DbContextOptions<DbBooking> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }
    }
}
