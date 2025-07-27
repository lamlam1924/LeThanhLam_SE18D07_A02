using BusinessObjects.Models;
using DataAccessLayer;

namespace BusinessLogicLayer.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        
        public List<BookingReservation> GetAllBookings()
        {
            return BookingReservationDAO.Get();
        }

        public List<BookingReservation> GetBookingsByCustomerId(int customerId)
        {
            return BookingReservationDAO.Search(customerId);
        }

        public BookingReservation GetBookingById(int id)
        {
            try
            {
                return BookingReservationDAO.Get().FirstOrDefault(b => b.BookingReservationId == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void AddBooking(BookingReservation booking)
        {
            BookingReservationDAO.Save(booking);
        }

        public void UpdateBooking(BookingReservation booking)
        {
            BookingReservationDAO.Update(booking);
        }

        public void CancelBooking(int id)
        {
            BookingReservationDAO.Delete(id);
        }

        public List<RoomInformation> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            try
            {
                var allRooms = RoomInformationDAO.Get();
                var availableRooms = new List<RoomInformation>();

                foreach (var room in allRooms ?? new List<RoomInformation>())
                {
                    if (room != null && IsRoomAvailable(room.RoomId, startDate, endDate))
                    {
                        availableRooms.Add(room);
                    }
                }

                return availableRooms;
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
                var bookings = BookingDetailDAO.Get();
                if (bookings == null) return true;
                var overlappingBookings = bookings.Where(b =>
                    b.RoomId == roomId &&
                    (
                        (b.StartDate.ToDateTime(TimeOnly.MinValue) < endDate && b.EndDate.ToDateTime(TimeOnly.MinValue) > startDate)
                        ||
                        (b.StartDate.ToDateTime(TimeOnly.MinValue) == startDate && b.EndDate.ToDateTime(TimeOnly.MinValue) == endDate)
                    )
                );
                return !overlappingBookings.Any(b => excludeBookingId == null || b.BookingReservationId != excludeBookingId);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
