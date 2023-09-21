using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            // throw new NotImplementedException();
            var newRoom = _context.Rooms.FirstOrDefault(room => room.RoomId == booking.RoomId);
            var roomUser = _context.Users.FirstOrDefault(user => user.Email == email);
            var hotel = _context.Hotels.FirstOrDefault(findHotel => findHotel.HotelId == newRoom!.HotelId);
            var city = _context.Cities.FirstOrDefault(cit => cit.CityId == hotel!.CityId);

            if (newRoom == null || booking.GuestQuant > newRoom.Capacity)
            {
                return null!;
            }

            var newBook = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = newRoom,
            };

            _context.Bookings.Add(newBook);
            _context.SaveChanges();

            var result = new BookingResponse
            {
                BookingId = newBook.BookingId,
                CheckIn = newBook.CheckIn,
                CheckOut = newBook.CheckOut,
                GuestQuant = newBook.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = newRoom.RoomId,
                    Name = newRoom.Name,
                    Capacity = newRoom.Capacity,
                    Image = newRoom.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = hotel!.HotelId,
                        Name = hotel!.Name,
                        Address = hotel!.Address,
                        CityId = hotel!.CityId,
                        CityName = newRoom.Hotel.City!.Name!,
                        State = newRoom.Hotel.City!.State!,
                    }
                },
            };

            return result;
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var booking = _context.Bookings
                .Include(b => b.Room)
                .ThenInclude(r => r!.Hotel)
                .ThenInclude(h => h!.City)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (user == null || booking == null)
                return null!;

            if (booking.UserId != user.UserId)
                return null!;

            return new BookingResponse
            {
                BookingId = booking.BookingId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = booking.Room!.RoomId,
                    Name = booking.Room.Name,
                    Capacity = booking.Room.Capacity,
                    Image = booking.Room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = booking.Room.Hotel!.HotelId,
                        Name = booking.Room.Hotel.Name,
                        Address = booking.Room.Hotel.Address,
                        CityId = booking.Room.Hotel.City!.CityId,
                        CityName = booking.Room.Hotel.City!.Name!,
                        State = booking.Room.Hotel.City!.State!
                    }
                }
            };
        }

        public Room GetRoomById(int RoomId)
        {
            throw new NotImplementedException();
        }

    }

}