using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using BusinessLogicLayer.Services;
using System.Windows.Input;

namespace LeThanhLamWPF.ViewModels
{
    public class RoomDialogViewModel : BaseViewModel
    {
        private RoomInformation _room;
        private bool _isEditMode;
        private readonly IRoomService _roomService;

        public RoomDialogViewModel(RoomInformation room = null)
        {
            _roomService = new RoomService();
            _isEditMode = room != null;
            _room = room ?? new RoomInformation();

            RoomTypes = new ObservableCollection<RoomType>(_roomService.GetAllRoomTypes());

            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        public RoomInformation Room
        {
            get => _room;
            set => SetProperty(ref _room, value);
        }

        public ObservableCollection<RoomType> RoomTypes { get; }

        public bool IsEditMode => _isEditMode;

        public string Title => _isEditMode ? "Edit Room" : "Add Room";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler<bool> RequestClose;

        private void Save()
        {
            RequestClose?.Invoke(this, true);
        }

        private void Cancel()
        {
            RequestClose?.Invoke(this, false);
        }
    }
}
