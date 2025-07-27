using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeThanhLamWPF.Views;
using BusinessObjects.Models;
using BusinessLogicLayer.Services;
using System.Windows.Input;
using System.Windows;

namespace LeThanhLamWPF.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<RoomInformation> _rooms;
        private Customer _selectedCustomer;
        private RoomInformation _selectedRoom;
        private string _customerSearchText;
        private string _roomSearchText;

        public AdminViewModel()
        {
            _customerService = new CustomerService();
            _roomService = new RoomService();

            LoadData();
            InitializeCommands();
        }

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }

        public ObservableCollection<RoomInformation> Rooms
        {
            get => _rooms;
            set => SetProperty(ref _rooms, value);
        }

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }

        public RoomInformation SelectedRoom
        {
            get => _selectedRoom;
            set => SetProperty(ref _selectedRoom, value);
        }

        public string CustomerSearchText
        {
            get => _customerSearchText;
            set => SetProperty(ref _customerSearchText, value);
        }

        public string RoomSearchText
        {
            get => _roomSearchText;
            set => SetProperty(ref _roomSearchText, value);
        }

        public ICommand AddCustomerCommand { get; private set; }
        public ICommand EditCustomerCommand { get; private set; }
        public ICommand DeleteCustomerCommand { get; private set; }
        public ICommand SearchCustomersCommand { get; private set; }
        public ICommand AddRoomCommand { get; private set; }
        public ICommand EditRoomCommand { get; private set; }
        public ICommand DeleteRoomCommand { get; private set; }
        public ICommand SearchRoomsCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        private void InitializeCommands()
        {
            AddCustomerCommand = new RelayCommand(_ => AddCustomer());
            EditCustomerCommand = new RelayCommand(_ => EditCustomer(), _ => SelectedCustomer != null);
            DeleteCustomerCommand = new RelayCommand(_ => DeleteCustomer(), _ => SelectedCustomer != null);
            SearchCustomersCommand = new RelayCommand(_ => SearchCustomers());
            AddRoomCommand = new RelayCommand(_ => AddRoom());
            EditRoomCommand = new RelayCommand(_ => EditRoom(), _ => SelectedRoom != null);
            DeleteRoomCommand = new RelayCommand(_ => DeleteRoom(), _ => SelectedRoom != null);
            SearchRoomsCommand = new RelayCommand(_ => SearchRooms());
            LogoutCommand = new RelayCommand(_ => Logout());
        }

        private void LoadData()
        {
            Customers = new ObservableCollection<Customer>(_customerService.GetAllCustomers());
            Rooms = new ObservableCollection<RoomInformation>(_roomService.GetAllRooms());
        }

        private void AddCustomer()
        {
            var dialog = new CustomerDialog();
            if (dialog.ShowDialog() == true)
            {
                _customerService.AddCustomer(dialog.Customer);
                LoadData();
            }
        }

        private void EditCustomer()
        {
            if (SelectedCustomer != null)
            {
                var dialog = new CustomerDialog(SelectedCustomer);
                if (dialog.ShowDialog() == true)
                {
                    _customerService.UpdateCustomer(dialog.Customer);
                    LoadData();
                }
            }
        }

        private void DeleteCustomer()
        {
            if (SelectedCustomer != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete customer '{SelectedCustomer.CustomerFullName}'?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _customerService.DeleteCustomer(SelectedCustomer.CustomerId);
                    LoadData();
                }
            }
        }

        private void SearchCustomers()
        {
            if (string.IsNullOrEmpty(CustomerSearchText))
            {
                Customers = new ObservableCollection<Customer>(_customerService.GetAllCustomers());
            }
            else
            {
                Customers = new ObservableCollection<Customer>(_customerService.SearchCustomers(CustomerSearchText));
            }
        }

        private void AddRoom()
        {
            var dialog = new RoomDialog();
            if (dialog.ShowDialog() == true)
            {
                _roomService.AddRoom(dialog.Room);
                LoadData();
            }
        }

        private void EditRoom()
        {
            if (SelectedRoom != null)
            {
                var dialog = new RoomDialog(SelectedRoom);
                if (dialog.ShowDialog() == true)
                {
                    _roomService.UpdateRoom(dialog.Room);
                    LoadData();
                }
            }
        }

        private void DeleteRoom()
        {
            if (SelectedRoom != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete room '{SelectedRoom.RoomNumber}'?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _roomService.DeleteRoom(SelectedRoom.RoomId);
                    LoadData();
                }
            }
        }

        private void SearchRooms()
        {
            if (string.IsNullOrEmpty(RoomSearchText))
            {
                Rooms = new ObservableCollection<RoomInformation>(_roomService.GetAllRooms());
            }
            else
            {
                Rooms = new ObservableCollection<RoomInformation>(_roomService.SearchRooms(RoomSearchText));
            }
        }

        private void Logout()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Application.Current.MainWindow?.Close();
        }
    }
}
