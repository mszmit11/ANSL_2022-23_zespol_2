using BOOKING.Models;
using Microsoft.EntityFrameworkCore;

namespace BOOKING
{
    public class DbBooking : DbContext
    {
        public DbBooking(DbContextOptions<DbBooking> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
