using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace DataAccessLayer
{
    public class BookingReservationDAO
    {
        public static List<BookingReservation> Get()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingReservations
                    .Include(br => br.Customer)
                    .Include(br => br.BookingDetails)
                        .ThenInclude(d => d.Room)
                            .ThenInclude(r => r.RoomType)
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"BookingReservationDAO Error: {e.InnerException?.Message ?? e.Message}");
            }
        }

        public static void Save(BookingReservation booking)
        {
            // Validate BookingDetails before saving
            if (booking.BookingDetails == null || booking.BookingDetails.Count == 0)
            {
                throw new Exception("Cannot save BookingReservation: BookingDetails is null or empty. You must select at least one room.");
            }
            try
            {
                using var context = new FuminiHotelManagementContext();
                context.BookingReservations.Add(booking);
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var log = new StringBuilder();
                log.AppendLine($"----- {DateTime.Now} -----");
                log.AppendLine($"DbUpdateException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    log.AppendLine($"InnerException: {ex.InnerException.Message}");
                }
                log.AppendLine($"StackTrace: {ex.StackTrace}");
                log.AppendLine($"BookingReservation: ");
                log.AppendLine($"  CustomerId: {booking.CustomerId}");
                log.AppendLine($"  BookingDate: {booking.BookingDate}");
                log.AppendLine($"  TotalPrice: {booking.TotalPrice}");
                log.AppendLine($"  BookingStatus: {booking.BookingStatus}");
                if (booking.BookingDetails != null)
                {
                    log.AppendLine($"  BookingDetails count: {booking.BookingDetails.Count}");
                    foreach (var detail in booking.BookingDetails)
                    {
                        log.AppendLine($"    RoomId: {detail.RoomId}, StartDate: {detail.StartDate}, EndDate: {detail.EndDate}, ActualPrice: {detail.ActualPrice}");
                    }
                }
                System.IO.File.AppendAllText("booking_error.log", log.ToString());
                throw;
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
                throw new Exception($"BookingReservationDAO Error: {e.InnerException?.Message ?? e.Message}");
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
                throw new Exception($"BookingReservationDAO Error: {e.InnerException?.Message ?? e.Message}");
            }
        }

        public static List<BookingReservation> Search(int customerId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingReservations
                    .Where(b => b.CustomerId == customerId)
                    .Include(b => b.Customer)
                    .Include(b => b.BookingDetails)
                        .ThenInclude(d => d.Room)
                            .ThenInclude(r => r.RoomType)
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"BookingReservationDAO Error: {e.InnerException?.Message ?? e.Message}");
            }
        }
    }
}
