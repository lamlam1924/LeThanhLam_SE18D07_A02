using BusinessObjects.Models;
using System;

namespace LeThanhLamWPF.ViewModels
{
    public class BookingViewModel
    {
        private readonly BookingReservation _booking;
        private readonly BookingDetail _detail;

        public BookingViewModel(BookingReservation booking)
        {
            _booking = booking;
            _detail = booking.BookingDetails.FirstOrDefault();
        }

        public int BookingReservationID => _booking.BookingReservationId;
        public string RoomNumber => _detail?.Room?.RoomNumber ?? "";
        public string RoomTypeName => _detail?.Room?.RoomType?.RoomTypeName ?? "";
        public DateTime ActualStartDate => _detail != null ? _detail.StartDate.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue;
        public DateTime ActualEndDate => _detail != null ? _detail.EndDate.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue;
        public decimal ActualPrice => _detail?.ActualPrice ?? 0;
        public string BookingStatusText
        {
            get
            {
                return _booking.BookingStatus switch
                {
                    1 => "Active",
                    2 => "Cancelled",
                    3 => "Completed",
                    _ => "Unknown"
                };
            }
        }
        public string CustomerFullName => _booking.Customer?.CustomerFullName ?? "";
        public DateTime BookingDate => _booking.BookingDate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue;
    }
}