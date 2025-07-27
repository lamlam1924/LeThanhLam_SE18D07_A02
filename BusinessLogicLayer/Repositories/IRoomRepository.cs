using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace BusinessLogicLayer.Repositories
{
    public interface IRoomRepository
    {
        List<RoomInformation> GetAllRooms();
        RoomInformation GetRoomById(int id);
        void AddRoom(RoomInformation room);
        void UpdateRoom(RoomInformation room);
        void DeleteRoom(int id);
        List<RoomInformation> SearchRooms(string searchTerm);
        List<RoomType> GetAllRoomTypes();
    }
}
