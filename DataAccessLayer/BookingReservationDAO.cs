using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class BookingReservationDAO
    {
        public static List<BookingReservation> Get()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingReservations.Include(br => br.BookingDetails).Include(br => br.Customer).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Save(BookingReservation booking)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                if (!context.BookingReservations.Any(b => b.BookingReservationId == booking.BookingReservationId))
                {
                    context.BookingReservations.Add(booking);
                }
                else
                {
                    throw new Exception($"BookingReservationId = {booking.BookingReservationId} existed!!! Please enter another ID");
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Update(BookingReservation booking)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var existingBooking = context.BookingReservations.FirstOrDefault(b => b.BookingReservationId == booking.BookingReservationId);

                if (existingBooking != null)
                {
                    existingBooking.BookingDate = booking.BookingDate;
                    existingBooking.TotalPrice = booking.TotalPrice;
                    existingBooking.CustomerId = booking.CustomerId;
                    existingBooking.BookingStatus = booking.BookingStatus;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Delete(int bookingReservationId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var booking = context.BookingReservations.FirstOrDefault(b => b.BookingReservationId == bookingReservationId);

                if (booking != null)
                {
                    context.BookingReservations.Remove(booking);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static List<BookingReservation> Search(int customerId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingReservations
                    .Where(b => b.CustomerId == customerId).Include(b => b.Customer).Include(b => b.BookingDetails)
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
