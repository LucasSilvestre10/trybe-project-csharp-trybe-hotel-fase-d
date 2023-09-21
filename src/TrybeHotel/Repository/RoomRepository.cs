using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            var result = _context.Rooms
                .Where(r => r.HotelId == HotelId)
                .ToList()
                .Select(r =>
                {
                    var hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == r.HotelId);
                    var cityName = _context.Cities
                        .Where(c => c.CityId == hotel!.CityId)
                        .FirstOrDefault();

                    return new RoomDto
                    {
                        RoomId = r.RoomId,
                        Name = r.Name,
                        Capacity = r.Capacity,
                        Image = r.Image,
                        Hotel = new HotelDto
                        {
                            HotelId = hotel!.HotelId,
                            Name = hotel.Name,
                            Address = hotel.Address,
                            CityId = hotel.CityId,
                            CityName = cityName!.Name,
                            State = cityName!.State
                        }
                    };
                });

            return result;
        }


        public RoomDto AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();

            var hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == room.HotelId);

            var cityName = _context.Cities.Where(c => c.CityId == hotel!.CityId).FirstOrDefault();

            return new RoomDto
            {
                RoomId = room.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = new HotelDto
                {
                    HotelId = hotel!.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityId = hotel.CityId,
                    CityName = cityName!.Name,
                    State = cityName!.State
                }
            };
        }


        public void DeleteRoom(int RoomId)
        {
            var result = _context.Rooms.FirstOrDefault(r => r.RoomId == RoomId);

            if (result == null)
                throw new Exception("Room not found");

            _context.Rooms.Remove(result);
            _context.SaveChanges();
        }
    }
}