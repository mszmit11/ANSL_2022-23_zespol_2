﻿using BOOKING.Data;
using BOOKING.Models;
using BOOKING.Services.Interface;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

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
            var product = _BookingContex.Products
               .Include(p => p.ImageUrls)
               .Include(p => p.ImageStorageNames)
               .SingleOrDefault(p => p.Id == id);

            if (product != null)
            {
                _BookingContex.ImageUrls.RemoveRange(product.ImageUrls);
                _BookingContex.ImageStorageNames.RemoveRange(product.ImageStorageNames);
                _BookingContex.Products.Remove(product);
                _BookingContex.SaveChanges();
            }

            return id;
        }

        public Product Get(int id)
        {
            var product = _BookingContex.Products.Include(i => i.ImageUrls)
              .FirstOrDefault(x => x.Id == id);

            return product!;
        }

        public List<Product> GetAll()
        {
            var products = _BookingContex.Products.Include(x=> x.ImageUrls).ToList();

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

        public int SaveReservation(Reservation reservation)
        {
            _BookingContex.Reservations.Add(reservation);

            if (_BookingContex.SaveChanges() > 0)
            {
                System.Console.WriteLine("SUKCES");
            };

            return reservation.Id;
        }

        public int DeleteReservation(int id)
        {
            var reservation = _BookingContex.Reservations.Find(id);
            _BookingContex.Reservations.Remove(reservation);
            _BookingContex.SaveChanges();

            return id;
        }

        public Reservation GetReservation(int id)
        {
            var reservation = _BookingContex.Reservations.Find(id);

            return reservation;
        }

        public List<Reservation> GetAllReservations()
        {
            var reservation = _BookingContex.Reservations.ToList();

            return reservation;
        }

        public List<Reservation> GetReservationId(int ProductId)
        {
            var reservation = _BookingContex.Reservations
                .Where(r => r.ProductId == ProductId)
                .ToList();

            return reservation;
        }

    }
}
