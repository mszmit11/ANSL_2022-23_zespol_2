using BOOKING.Models;

namespace BOOKING.Services.Interface
{
    public interface IBookingService
    {
        int Save(Product product);
        List<Product> GetAll();
        Product Get(int id);
        int Delete(int id);
        int SaveReservation(Reservation reservation);
        List<Reservation> GetAllReservations();
        Reservation GetReservation(int id);
        int DeleteReservation(int id);
    }
}
