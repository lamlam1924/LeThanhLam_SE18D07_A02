using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace DataAccessLayer
{
    public class RoomTyoeDAO
    {
        public static List<RoomType> Get()
        {

            var listRoomTypes = new List<RoomType>();
            try
            {
                using var context = new FuminiHotelManagementContext();
                listRoomTypes = context.RoomTypes.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listRoomTypes;
        }

        public static void Save(RoomType roomType)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                if (!context.RoomTypes.Any(r => r.RoomTypeId == roomType.RoomTypeId))
                {
                    context.RoomTypes.Add(roomType);
                }
                else
                {
                    throw new Exception($"RoomId = {roomType.RoomTypeId} existed!!! Please enter another ID");
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Update(RoomType roomType)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var existingRoomType = context.RoomTypes.FirstOrDefault(r => r.RoomTypeId == roomType.RoomTypeId);

                if (existingRoomType != null)
                {
                    existingRoomType.TypeDescription = roomType.TypeDescription;
                    existingRoomType.TypeNote = roomType.TypeNote;
                    existingRoomType.RoomTypeName = roomType.RoomTypeName;
                    existingRoomType.RoomInformations = roomType.RoomInformations;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Delete(int roomTypeId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var roomType = context.RoomTypes.FirstOrDefault(r => r.RoomTypeId == roomTypeId);

                if (roomType != null)
                {
                    context.RoomTypes.Remove(roomType);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static List<RoomType> Search(string roomTypeId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var roomTypes = context.RoomTypes
                    .Where(r => r.RoomTypeId.Equals(roomTypeId))
                    .ToList();
                return roomTypes;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
