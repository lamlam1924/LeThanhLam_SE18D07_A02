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
using System.ComponentModel;
using LeThanhLamWPF.ViewModels;

namespace LeThanhLamWPF.Views
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window, INotifyPropertyChanged
    {
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;

        private int _totalCustomers;
        public int TotalCustomers
        {
            get => _totalCustomers;
            set { _totalCustomers = value; OnPropertyChanged(nameof(TotalCustomers)); }
        }

        private int _totalRooms;
        public int TotalRooms
        {
            get => _totalRooms;
            set { _totalRooms = value; OnPropertyChanged(nameof(TotalRooms)); }
        }

        private int _totalBookings;
        public int TotalBookings
        {
            get => _totalBookings;
            set { _totalBookings = value; OnPropertyChanged(nameof(TotalBookings)); }
        }

        private decimal _totalRevenue;
        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set { _totalRevenue = value; OnPropertyChanged(nameof(TotalRevenue)); }
        }

        public AdminWindow()
        {
            InitializeComponent();
            DataContext = this;

            try
            {
                _customerService = new CustomerService();
                _roomService = new RoomService();
                _bookingService = new BookingService();

                // Load data after the window is fully loaded
                this.Loaded += AdminWindow_Loaded;

                // Giả lập dữ liệu báo cáo, bạn thay bằng truy vấn thực tế từ DB
                TotalCustomers = 120;
                TotalRooms = 45;
                TotalBookings = 320;
                TotalRevenue = 15000000;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize admin window: {ex.Message}", "Initialization Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void AdminWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load initial data: {ex.Message}", "Data Loading Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadData()
        {
            try
            {
                // Load customers
                var customers = _customerService?.GetAllCustomers() ?? new List<Customer>();
                if (CustomersDataGrid != null)
                    CustomersDataGrid.ItemsSource = customers;

                // Load rooms
                var rooms = _roomService?.GetAllRooms() ?? new List<RoomInformation>();
                if (RoomsDataGrid != null)
                    RoomsDataGrid.ItemsSource = rooms;

                // Load bookings
                var bookings = _bookingService?.GetAllBookings() ?? new List<BookingReservation>();
                LoadBookings();

                // Cập nhật số liệu báo cáo
                TotalCustomers = customers.Count;
                TotalRooms = rooms.Count;
                TotalBookings = bookings.Count;
                TotalRevenue = bookings.Sum(b => b.TotalPrice ?? 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load data: {ex.Message}", "Data Loading Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadBookings()
        {
            try
            {
                if (_bookingService == null || BookingsDataGrid == null)
                {
                    return;
                }

                var bookings = _bookingService.GetAllBookings();

                // Apply filter only if BookingStatusFilter is available and has a selection
                if (BookingStatusFilter != null && BookingStatusFilter.SelectedIndex > 0)
                {
                    int statusFilter = BookingStatusFilter.SelectedIndex;
                    bookings = bookings?.Where(b => b != null && b.BookingStatus == statusFilter).ToList() ?? new List<BookingReservation>();
                }

                // Chuyển sang ViewModel để binding đúng
                var bookingViewModels = bookings.Select(b => new BookingViewModel(b)).ToList();
                BookingsDataGrid.ItemsSource = bookingViewModels;
            }
            catch
            {
                if (BookingsDataGrid != null)
                    BookingsDataGrid.ItemsSource = new List<BookingViewModel>();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}", "Logout Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchCustomersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_customerService == null || CustomersDataGrid == null) return;

                string searchTerm = CustomerSearchTextBox?.Text?.Trim() ?? "";
                if (string.IsNullOrEmpty(searchTerm))
                {
                    CustomersDataGrid.ItemsSource = _customerService.GetAllCustomers();
                }
                else
                {
                    CustomersDataGrid.ItemsSource = _customerService.SearchCustomers(searchTerm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during customer search: {ex.Message}", "Search Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_customerService == null) return;

                var dialog = new CustomerDialog();
                if (dialog.ShowDialog() == true && dialog.Customer != null)
                {
                    _customerService.AddCustomer(dialog.Customer);
                    LoadCustomerData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}", "Add Customer Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_customerService == null || CustomersDataGrid == null) return;

                var selectedCustomer = CustomersDataGrid.SelectedItem as Customer;
                if (selectedCustomer != null)
                {
                    var dialog = new CustomerDialog(selectedCustomer);
                    if (dialog.ShowDialog() == true && dialog.Customer != null)
                    {
                        _customerService.UpdateCustomer(dialog.Customer);
                        LoadCustomerData();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a customer to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing customer: {ex.Message}", "Edit Customer Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_customerService == null || CustomersDataGrid == null) return;

                var selectedCustomer = CustomersDataGrid.SelectedItem as Customer;
                if (selectedCustomer != null)
                {
                    var result = MessageBox.Show($"Are you sure you want to delete customer '{selectedCustomer.CustomerFullName}'?",
                        "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        _customerService.DeleteCustomer(selectedCustomer.CustomerId);
                        LoadCustomerData();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a customer to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting customer: {ex.Message}", "Delete Customer Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_roomService == null || RoomsDataGrid == null) return;

                string searchTerm = RoomSearchTextBox?.Text?.Trim() ?? "";
                if (string.IsNullOrEmpty(searchTerm))
                {
                    RoomsDataGrid.ItemsSource = _roomService.GetAllRooms();
                }
                else
                {
                    RoomsDataGrid.ItemsSource = _roomService.SearchRooms(searchTerm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during room search: {ex.Message}", "Search Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_roomService == null) return;

                var dialog = new RoomDialog();
                if (dialog.ShowDialog() == true && dialog.Room != null)
                {
                    _roomService.AddRoom(dialog.Room);
                    LoadRoomData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding room: {ex.Message}", "Add Room Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditRoomButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_roomService == null || RoomsDataGrid == null) return;

                var selectedRoom = RoomsDataGrid.SelectedItem as RoomInformation;
                if (selectedRoom != null)
                {
                    var dialog = new RoomDialog(selectedRoom);
                    if (dialog.ShowDialog() == true && dialog.Room != null)
                    {
                        _roomService.UpdateRoom(dialog.Room);
                        LoadRoomData();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a room to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing room: {ex.Message}", "Edit Room Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteRoomButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_roomService == null || RoomsDataGrid == null) return;

                var selectedRoom = RoomsDataGrid.SelectedItem as RoomInformation;
                if (selectedRoom != null)
                {
                    var result = MessageBox.Show($"Are you sure you want to delete room '{selectedRoom.RoomNumber}'?",
                        "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        _roomService.DeleteRoom(selectedRoom.RoomId);
                        LoadRoomData();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a room to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting room: {ex.Message}", "Delete Room Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BookingStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Only load bookings if the window is fully loaded
                if (this.IsLoaded)
                {
                    LoadBookings();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering bookings: {ex.Message}", "Filter Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Helper methods to load specific data
        private void LoadCustomerData()
        {
            try
            {
                if (_customerService != null && CustomersDataGrid != null)
                    CustomersDataGrid.ItemsSource = _customerService.GetAllCustomers();
            }
            catch
            {
                // Silent fail for helper method
            }
        }

        private void LoadRoomData()
        {
            try
            {
                if (_roomService != null && RoomsDataGrid != null)
                    RoomsDataGrid.ItemsSource = _roomService.GetAllRooms();
            }
            catch
            {
                // Silent fail for helper method
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
