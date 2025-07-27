using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class RoomInformationDAO
    {

        public static List<RoomInformation> Get()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.RoomInformations.Include(r => r.RoomType).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Save(RoomInformation room)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                if (!context.RoomInformations.Any(r => r.RoomId == room.RoomId))
                {
                    context.RoomInformations.Add(room);
                }
                else
                {
                    throw new Exception($"RoomId = {room.RoomId} existed!!! Please enter another ID");
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Update(RoomInformation room)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var existingRoom = context.RoomInformations.FirstOrDefault(r => r.RoomId == room.RoomId);

                if (existingRoom != null)
                {
                    existingRoom.RoomNumber = room.RoomNumber;
                    existingRoom.RoomDetailDescription = room.RoomDetailDescription;
                    existingRoom.RoomMaxCapacity = room.RoomMaxCapacity;
                    existingRoom.RoomTypeId = room.RoomTypeId;
                    existingRoom.RoomStatus = room.RoomStatus;
                    existingRoom.RoomPricePerDay = room.RoomPricePerDay;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Delete(int roomId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var room = context.RoomInformations.FirstOrDefault(r => r.RoomId == roomId);

                if (room != null)
                {
                    context.RoomInformations.Remove(room);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
