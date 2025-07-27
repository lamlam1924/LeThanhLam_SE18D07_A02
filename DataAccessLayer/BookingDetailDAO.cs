using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class BookingDetailDAO
    {
        public static List<BookingDetail> Get()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingDetails.Include(bd => bd.BookingReservation).Include(bd => bd.Room).Include(bd => bd.BookingReservation.Customer).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Save(BookingDetail bookingDetail)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                if (!context.BookingDetails.Any(b => b.BookingReservationId == bookingDetail.BookingReservationId && b.RoomId == bookingDetail.RoomId))
                {
                    context.BookingDetails.Add(bookingDetail);
                }
                else
                {
                    throw new Exception($"BookingReservationId = {bookingDetail.BookingReservationId} and RoomId = {bookingDetail.RoomId} already exists!!!");
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Update(BookingDetail bookingDetail)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var existingBookingDetail = context.BookingDetails
                    .FirstOrDefault(b => b.BookingReservationId == bookingDetail.BookingReservationId && b.RoomId == bookingDetail.RoomId);

                if (existingBookingDetail != null)
                {
                    existingBookingDetail.StartDate = bookingDetail.StartDate;
                    existingBookingDetail.EndDate = bookingDetail.EndDate;
                    existingBookingDetail.ActualPrice = bookingDetail.ActualPrice;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Delete(int bookingReservationId, int roomId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var bookingDetail = context.BookingDetails
                    .FirstOrDefault(b => b.BookingReservationId == bookingReservationId && b.RoomId == roomId);

                if (bookingDetail != null)
                {
                    context.BookingDetails.Remove(bookingDetail);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static List<BookingDetail> Search(int customerId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingDetails
                    .Where(b => b.BookingReservation.CustomerId == customerId).Include(bd => bd.BookingReservation).Include(bd => bd.Room)
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
