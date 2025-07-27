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

namespace LeThanhLamWPF.Views
{
    /// <summary>
    /// Interaction logic for CustomerDialog.xaml
    /// </summary>
    public partial class CustomerDialog : Window
    {
        public Customer Customer { get; private set; }

        public CustomerDialog(Customer customer = null)
        {
            InitializeComponent();

            if (customer != null)
            {
                Customer = new Customer
                {
                    CustomerId = customer.CustomerId,
                    CustomerFullName = customer.CustomerFullName,
                    EmailAddress = customer.EmailAddress,
                    Telephone = customer.Telephone,
                    CustomerBirthday = customer.CustomerBirthday,
                    Password = customer.Password,
                    CustomerStatus = customer.CustomerStatus
                };

                FullNameTextBox.Text = Customer.CustomerFullName;
                EmailTextBox.Text = Customer.EmailAddress;
                PhoneTextBox.Text = Customer.Telephone;
                BirthdayDatePicker.SelectedDate = Customer.CustomerBirthday?.ToDateTime(TimeOnly.MinValue);
                PasswordTextBox.Text = Customer.Password;

                Title = "Edit Customer";
            }
            else
            {
                Customer = new Customer();
                BirthdayDatePicker.SelectedDate = DateTime.Now;
                Title = "Add Customer";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FullNameTextBox.Text) ||
                string.IsNullOrEmpty(EmailTextBox.Text) ||
                string.IsNullOrEmpty(PhoneTextBox.Text) ||
                string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Customer.CustomerFullName = FullNameTextBox.Text;
            Customer.EmailAddress = EmailTextBox.Text;
            Customer.Telephone = PhoneTextBox.Text;
            Customer.CustomerBirthday = BirthdayDatePicker.SelectedDate.HasValue
                ? DateOnly.FromDateTime(BirthdayDatePicker.SelectedDate.Value)
                : DateOnly.FromDateTime(DateTime.Now);
            Customer.Password = PasswordTextBox.Text;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
