using BusinessObjects.Models;

namespace BusinessLogicLayer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerService _customerService;

        public AuthenticationService()
        {
            try
            {
                _customerService = new CustomerService();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize CustomerService", ex);
            }
        }

        public bool AuthenticateAdmin(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return false;

                string adminEmail = "admin@FUMiniHotelSystem.com";
                string adminPassword = "@@abc123@@";

                return string.Equals(email, adminEmail, StringComparison.OrdinalIgnoreCase) &&
                       string.Equals(password, adminPassword, StringComparison.Ordinal);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Customer AuthenticateCustomer(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return null;

                if (_customerService == null)
                    return null;

                var customer = _customerService.GetCustomerByEmail(email);
                if (customer != null && !string.IsNullOrEmpty(customer.Password) &&
                    string.Equals(customer.Password, password, StringComparison.Ordinal))
                {
                    return customer;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
