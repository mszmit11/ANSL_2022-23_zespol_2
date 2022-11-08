using BOOKING.Models;
using BOOKING.Services.Interface;

namespace BOOKING.Services
{
    public class BookingService : IBookingService
    {
        private readonly DbBooking _BookingContex;

        public BookingService(DbBooking context)
        {
            _BookingContex = context;
        }

        public int Delete(int id)
        {
            var product = _BookingContex.Products.Find(id);
            _BookingContex.Products.Remove(product);
            _BookingContex.SaveChanges();

            return id;
        }

        public Product Get(int id)
        {
            var product = _BookingContex.Products.Find(id);

            return product;
        }

        public List<Product> GetAll()
        {
            var products = _BookingContex.Products.ToList();

            return products;
        }

        public int Save(Product product)
        {
            _BookingContex.Products.Add(product);

            if (_BookingContex.SaveChanges() > 0)
            {
                System.Console.WriteLine("SUKCES");
            };

            return product.Id;
        }
    }
}
