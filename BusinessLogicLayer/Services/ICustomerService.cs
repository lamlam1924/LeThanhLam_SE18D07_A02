using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace BusinessLogicLayer.Services
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        Customer GetCustomerByEmail(string email);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
        List<Customer> SearchCustomers(string searchTerm);
        bool ValidateCustomer(Customer customer);
    }
}
