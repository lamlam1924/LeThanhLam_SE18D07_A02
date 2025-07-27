using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using System.Windows.Input;

namespace LeThanhLamWPF.ViewModels
{
    public class CustomerDialogViewModel : BaseViewModel
    {
        private Customer _customer;
        private bool _isEditMode;

        public CustomerDialogViewModel(Customer customer = null)
        {
            _isEditMode = customer != null;
            _customer = customer ?? new Customer();

            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        public Customer Customer
        {
            get => _customer;
            set => SetProperty(ref _customer, value);
        }

        public bool IsEditMode => _isEditMode;

        public string Title => _isEditMode ? "Edit Customer" : "Add Customer";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler<bool> RequestClose;

        private void Save()
        {
            // Validation would go here
            RequestClose?.Invoke(this, true);
        }

        private void Cancel()
        {
            RequestClose?.Invoke(this, false);
        }
    }
}
