using BusinessLogicLayer.Services;
using BusinessObjects.Models;
using System.Windows;
using LeThanhLamWPF.Views;

namespace LeThanhLamWPF.Views
{
    public partial class CustomerWindow : Window
    {
        private Customer _currentCustomer;
        private readonly ICustomerService _customerService;
        private readonly IBookingService _bookingService;

        public CustomerWindow(Customer customer)
        {
            InitializeComponent();
            _currentCustomer = customer;
            _customerService = new CustomerService();
            _bookingService = new BookingService();
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            WelcomeTextBlock.Text = $"Welcome, {_currentCustomer.CustomerFullName}";
            FullNameTextBlock.Text = _currentCustomer.CustomerFullName;
            EmailTextBlock.Text = _currentCustomer.EmailAddress;
            PhoneTextBlock.Text = _currentCustomer.Telephone;
            BirthdayTextBlock.Text = _currentCustomer.CustomerBirthday != null
                ? _currentCustomer.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue).ToString("dd/MM/yyyy")
                : DateTime.Now.ToString("dd/MM/yyyy");
            CustomerIDTextBlock.Text = _currentCustomer.CustomerId.ToString();
        }

        private void UpdateProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CustomerDialog(_currentCustomer);
            if (dialog.ShowDialog() == true)
            {
                _customerService.UpdateCustomer(dialog.Customer);
                _currentCustomer = dialog.Customer;
                LoadCustomerData();
                MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void NewBookingButton_Click(object sender, RoutedEventArgs e)
        {
            var bookingDialog = new BookingDialog(_currentCustomer);
            bookingDialog.Owner = this;
            bookingDialog.ShowDialog();
        }

        private void ViewBookingHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var historyWindow = new BookingHistoryWindow(_currentCustomer);
            historyWindow.Owner = this;
            historyWindow.ShowDialog();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}