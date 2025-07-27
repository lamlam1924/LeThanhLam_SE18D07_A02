using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessLayer;

namespace BusinessLogicLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        public List<Customer> GetAllCustomers()
        {
            try
            {
                return CustomerDAO.Get().Where(c => c != null && c.CustomerStatus == 1).ToList();
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
                return CustomerDAO.GetCustomerbyId(id);
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
                if (string.IsNullOrEmpty(email))
                    return null;
                return CustomerDAO.Get().FirstOrDefault(c => c != null && !string.IsNullOrEmpty(c.EmailAddress)
                    && string.Equals(c.EmailAddress, email, StringComparison.OrdinalIgnoreCase)
                    && c.CustomerStatus == 1);
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
                if (customer == null) return;
                customer.CustomerStatus = 1;
                CustomerDAO.Save(customer);
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
                if (customer == null) return;
                CustomerDAO.Update(customer);
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
                CustomerDAO.Delete(id);
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
                if (string.IsNullOrEmpty(searchTerm))
                    return GetAllCustomers();
                return CustomerDAO.Search(searchTerm);
            }
            catch (Exception)
            {
                return new List<Customer>();
            }
        }
    }
}
