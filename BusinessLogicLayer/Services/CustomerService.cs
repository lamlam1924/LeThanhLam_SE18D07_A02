using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using BusinessLogicLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }

        public List<Customer> GetAllCustomers()
        {
            try
            {
                return _customerRepository?.GetAllCustomers() ?? new List<Customer>();
            }
            catch (Exception)
            {
                return new List<Customer>();
            }
        }

        public Customer GetCustomerById(int id)
        {
            try
            {
                if (id <= 0) return null;
                return _customerRepository?.GetCustomerById(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Customer GetCustomerByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) return null;
                return _customerRepository?.GetCustomerByEmail(email);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void AddCustomer(Customer customer)
        {
            try
            {
                if (ValidateCustomer(customer))
                {
                    _customerRepository?.AddCustomer(customer);
                }
            }
            catch (Exception)
            {
                // Log error or handle as needed
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            try
            {
                if (ValidateCustomer(customer))
                {
                    _customerRepository?.UpdateCustomer(customer);
                }
            }
            catch (Exception)
            {
                // Log error or handle as needed
            }
        }

        public void DeleteCustomer(int id)
        {
            try
            {
                if (id > 0)
                {
                    _customerRepository?.DeleteCustomer(id);
                }
            }
            catch (Exception)
            {
                // Log error or handle as needed
            }
        }

        public List<Customer> SearchCustomers(string searchTerm)
        {
            try
            {
                return _customerRepository?.SearchCustomers(searchTerm) ?? new List<Customer>();
            }
            catch (Exception)
            {
                return new List<Customer>();
            }
        }

        public bool ValidateCustomer(Customer customer)
        {
            if (customer == null) return false;
            if (string.IsNullOrEmpty(customer.CustomerFullName)) return false;
            if (string.IsNullOrEmpty(customer.EmailAddress)) return false;
            if (string.IsNullOrEmpty(customer.Telephone)) return false;
            if (string.IsNullOrEmpty(customer.Password)) return false;

            return true;
        }
    }
}
