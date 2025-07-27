using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace BusinessLogicLayer.Repositories
{
    public interface IBookingRepository
    {
        List<BookingReservation> GetAllBookings();
        List<BookingReservation> GetBookingsByCustomerId(int customerId);
        BookingReservation GetBookingById(int id);
        void AddBooking(BookingReservation booking);
        void UpdateBooking(BookingReservation booking);
        void CancelBooking(int id);
        List<RoomInformation> GetAvailableRooms(DateTime startDate, DateTime endDate);
        bool IsRoomAvailable(int roomId, DateTime startDate, DateTime endDate, int? excludeBookingId = null);
    }
}
