using BOOKING.Models;

namespace BOOKING.Services.Interface
{
    public interface IBookingService
    {
        int Save(Product product);
        List<Product> GetAll();
        Product Get(int id);
        int Delete(int id);
    }
}
