using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace DataAccessLayer
{
    public class CustomerDAO
    {
        public static List<Customer> Get()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.Customers.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Customer GetCustomerbyId(int customerId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Save(Customer customer)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                if (!context.Customers.Any(c => c.CustomerId == customer.CustomerId))
                {
                    context.Customers.Add(customer);
                }
                else
                {
                    throw new Exception($"CustomerId = {customer.CustomerId} existed!!! Please enter another ID");
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Update(Customer customer)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var existingCustomer = context.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);

                if (existingCustomer != null)
                {
                    existingCustomer.CustomerFullName = customer.CustomerFullName;
                    existingCustomer.Telephone = customer.Telephone;
                    existingCustomer.EmailAddress = customer.EmailAddress;
                    existingCustomer.CustomerBirthday = customer.CustomerBirthday;
                    existingCustomer.CustomerStatus = customer.CustomerStatus;
                    existingCustomer.Password = customer.Password;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Delete(int customerId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var customer = context.Customers.FirstOrDefault(c => c.CustomerId == customerId);

                if (customer != null)
                {
                    context.Customers.Remove(customer);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static List<Customer> Search(string keyword)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.Customers
                    .Where(c => c.CustomerFullName.Contains(keyword) || c.EmailAddress.Contains(keyword))
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
