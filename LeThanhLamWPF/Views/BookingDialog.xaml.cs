using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessObjects.Models;
using BusinessLogicLayer.Services;

namespace LeThanhLamWPF.Views
{
    /// <summary>
    /// Interaction logic for BookingDialog.xaml
    /// </summary>
    public partial class BookingDialog : Window
    {
        private readonly IBookingService _bookingService;
        private readonly Customer _customer;
        public BookingReservation Booking { get; private set; }

        public BookingDialog(Customer customer)
        {
            InitializeComponent();
            _bookingService = new BookingService();
            _customer = customer;
            Booking = new BookingReservation
            {
                CustomerId = customer.CustomerId,
                Customer = customer
            };

            // Set minimum dates
            StartDatePicker.DisplayDateStart = DateTime.Today;
            EndDatePicker.DisplayDateStart = DateTime.Today.AddDays(1);

            // Set default dates
            StartDatePicker.SelectedDate = DateTime.Today.AddDays(1);
            EndDatePicker.SelectedDate = DateTime.Today.AddDays(2);

            // Load available rooms
            LoadAvailableRooms();
        }

        private void LoadAvailableRooms()
        {
            if (StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                var startDate = StartDatePicker.SelectedDate.Value;
                var endDate = EndDatePicker.SelectedDate.Value;

                if (startDate < endDate)
                {
                    var availableRooms = _bookingService.GetAvailableRooms(startDate, endDate);
                    RoomsDataGrid.ItemsSource = availableRooms;

                    // Update nights count
                    int nights = (endDate - startDate).Days;
                    NightsTextBlock.Text = $"Nights: {nights}";

                    // Clear selection
                    RoomsDataGrid.SelectedItem = null;
                    BookButton.IsEnabled = false;
                    PricePerNightTextBlock.Text = "Price per night: $0.00";
                    TotalPriceTextBlock.Text = "Total: $0.00";
                }
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ensure end date is after start date
            if (StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                if (EndDatePicker.SelectedDate <= StartDatePicker.SelectedDate)
                {
                    EndDatePicker.SelectedDate = StartDatePicker.SelectedDate.Value.AddDays(1);
                }
            }

            LoadAvailableRooms();
        }

        private void RoomsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRoom = RoomsDataGrid.SelectedItem as RoomInformation;
            if (selectedRoom != null && StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                var startDate = StartDatePicker.SelectedDate.Value;
                var endDate = EndDatePicker.SelectedDate.Value;
                int nights = (endDate - startDate).Days;

                decimal pricePerNight = selectedRoom?.RoomPricePerDay ?? 0;
                decimal totalPrice = pricePerNight * nights;

                PricePerNightTextBlock.Text = $"Price per night: {pricePerNight:C}";
                TotalPriceTextBlock.Text = $"Total: {totalPrice:C}";

                BookButton.IsEnabled = true;

                // Update booking object
                // Booking.RoomInformation = selectedRoom;
                Booking.TotalPrice = totalPrice;

                // Tạo mới BookingDetail cho booking này
                Booking.BookingDetails = new List<BookingDetail>
                {
                    new BookingDetail
                    {
                        RoomId = selectedRoom.RoomId,
                        StartDate = DateOnly.FromDateTime(startDate),
                        EndDate = DateOnly.FromDateTime(endDate),
                        ActualPrice = totalPrice,
                        Room = selectedRoom
                    }
                };
            }
            else
            {
                BookButton.IsEnabled = false;
                PricePerNightTextBlock.Text = "Price per night: $0.00";
                TotalPriceTextBlock.Text = "Total: $0.00";
            }
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            var detail = Booking.BookingDetails.FirstOrDefault();
            if (_bookingService.AddBooking(Booking) && detail != null)
            {
                MessageBox.Show($"Room {detail.Room.RoomNumber} booked successfully from " +
                               $"{detail.StartDate.ToString("dd/MM/yyyy")} to {detail.EndDate.ToString("dd/MM/yyyy")}.",
                               "Booking Confirmed", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Unable to complete booking. Please try again.",
                               "Booking Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
