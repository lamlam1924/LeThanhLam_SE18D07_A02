using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using BusinessLogicLayer.Repositories;
using System.Windows;

namespace BusinessLogicLayer.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService()
        {
            _bookingRepository = new BookingRepository();
        }

        public List<BookingReservation> GetAllBookings()
        {
            try
            {
                return _bookingRepository?.GetAllBookings() ?? new List<BookingReservation>();
            }
            catch (Exception)
            {
                return new List<BookingReservation>();
            }
        }

        public List<BookingReservation> GetBookingsByCustomerId(int customerId)
        {
            try
            {
                if (customerId <= 0) return new List<BookingReservation>();
                return _bookingRepository?.GetBookingsByCustomerId(customerId) ?? new List<BookingReservation>();
            }
            catch (Exception)
            {
                return new List<BookingReservation>();
            }
        }

        public BookingReservation GetBookingById(int id)
        {
            try
            {
                if (id <= 0) return null;
                return _bookingRepository?.GetBookingById(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool AddBooking(BookingReservation booking)
        {
            if (!ValidateBooking(booking)) return false;
            try
            {
                _bookingRepository.AddBooking(booking);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool UpdateBooking(BookingReservation booking)
        {
            try
            {
                if (!ValidateBooking(booking))
                    return false;

                var detail = booking.BookingDetails.FirstOrDefault();
                if (detail == null) return false;

                if (!IsRoomAvailable(detail.RoomId, detail.StartDate.ToDateTime(TimeOnly.MinValue), detail.EndDate.ToDateTime(TimeOnly.MinValue), booking.BookingReservationId))
                    return false;

                _bookingRepository?.UpdateBooking(booking);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CancelBooking(int id)
        {
            try
            {
                if (id <= 0) return false;

                var booking = _bookingRepository?.GetBookingById(id);
                if (booking == null)
                    return false;

                _bookingRepository?.CancelBooking(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<RoomInformation> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate >= endDate) return new List<RoomInformation>();
                return _bookingRepository?.GetAvailableRooms(startDate, endDate) ?? new List<RoomInformation>();
            }
            catch (Exception)
            {
                return new List<RoomInformation>();
            }
        }

        public bool IsRoomAvailable(int roomId, DateTime startDate, DateTime endDate, int? excludeBookingId = null)
        {
            try
            {
                if (roomId <= 0 || startDate >= endDate) return false;
                return _bookingRepository?.IsRoomAvailable(roomId, startDate, endDate, excludeBookingId) ?? false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ValidateBooking(BookingReservation booking)
        {
            if (booking == null) return false;
            if (booking.CustomerId <= 0) return false;
            var detail = booking.BookingDetails.FirstOrDefault();
            if (detail == null) return false;
            if (detail.RoomId <= 0) return false;
            var startDate = detail.StartDate.ToDateTime(TimeOnly.MinValue);
            var endDate = detail.EndDate.ToDateTime(TimeOnly.MinValue);
            if (startDate >= endDate) return false;
            if (startDate < DateTime.Today) return false;
            return true;
        }
    }
}
