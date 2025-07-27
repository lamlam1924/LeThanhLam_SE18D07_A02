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
using BusinessLogicLayer.Services;

namespace LeThanhLamWPF.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginWindow()
        {
            InitializeComponent();

            try
            {
                _authenticationService = new AuthenticationService();
                // Set focus to email textbox
                EmailTextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize login system: {ex.Message}", "Initialization Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = EmailTextBox?.Text?.Trim() ?? "";
                string password = PasswordBox?.Password ?? "";

                // Clear previous styling
                if (EmailTextBox != null)
                    EmailTextBox.BorderBrush = System.Windows.Media.Brushes.LightGray;
                if (PasswordBox != null)
                    PasswordBox.BorderBrush = System.Windows.Media.Brushes.LightGray;

                // Validation
                if (string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Please enter your email address.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    if (EmailTextBox != null)
                    {
                        EmailTextBox.BorderBrush = System.Windows.Media.Brushes.Red;
                        EmailTextBox.Focus();
                    }
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter your password.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    if (PasswordBox != null)
                    {
                        PasswordBox.BorderBrush = System.Windows.Media.Brushes.Red;
                        PasswordBox.Focus();
                    }
                    return;
                }

                if (_authenticationService == null)
                {
                    MessageBox.Show("Authentication service is not available.", "System Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Check admin login first
                if (_authenticationService.AuthenticateAdmin(email, password))
                {
                    MessageBox.Show("Admin login successful!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    var adminWindow = new AdminWindow();
                    adminWindow.Show();
                    this.Close();
                    return;
                }

                // Check customer login
                var customer = _authenticationService.AuthenticateCustomer(email, password);
                if (customer != null)
                {
                    MessageBox.Show($"Welcome {customer.CustomerFullName}!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    var customerWindow = new CustomerWindow(customer);
                    customerWindow.Show();
                    this.Close();
                    return;
                }

                // Login failed
                MessageBox.Show("Invalid email or password. Please check your credentials and try again.",
                    "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);

                // Clear password field
                if (PasswordBox != null)
                {
                    PasswordBox.Clear();
                    EmailTextBox?.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Handle Enter key press
        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    LoginButton_Click(this, new RoutedEventArgs());
                }
                base.OnKeyDown(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
