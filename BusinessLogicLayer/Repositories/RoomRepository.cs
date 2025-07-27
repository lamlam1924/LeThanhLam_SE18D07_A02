using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessLayer;

namespace BusinessLogicLayer.Repositories
{
    public class RoomRepository : IRoomRepository
    {

        public List<RoomInformation> GetAllRooms()
        {
            return DataAccessLayer.RoomInformationDAO.Get().Where(r => r.RoomStatus == 1).ToList();
        }

        public RoomInformation GetRoomById(int id)
        {
            return DataAccessLayer.RoomInformationDAO.Get().FirstOrDefault(r => r.RoomId == id && r.RoomStatus == 1);
        }

        public void AddRoom(RoomInformation room)
        {
            room.RoomStatus = 1;
            DataAccessLayer.RoomInformationDAO.Save(room);
        }

        public void UpdateRoom(RoomInformation room)
        {
            DataAccessLayer.RoomInformationDAO.Update(room);
        }

        public void DeleteRoom(int id)
        {
            DataAccessLayer.RoomInformationDAO.Delete(id);
        }

        public List<RoomInformation> SearchRooms(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAllRooms();
            return DataAccessLayer.RoomInformationDAO.Get().Where(r => r.RoomStatus == 1 &&
                (r.RoomNumber.ToLower().Contains(searchTerm.ToLower()) ||
                 r.RoomDetailDescription.ToLower().Contains(searchTerm.ToLower()))).ToList();
        }

        public List<RoomType> GetAllRoomTypes()
        {
            return DataAccessLayer.RoomTyoeDAO.Get();
        }
    }
}
