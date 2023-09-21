using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<HotelDto> GetHotels()
        {
            var hotels = _context.Hotels.Select(hotel => new HotelDto
            {
                HotelId = hotel.HotelId,
                CityId = hotel.CityId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityName = _context.Cities.Where(city => city.CityId == hotel.CityId).Select(city => city.Name).FirstOrDefault(),
                State = _context.Cities.Where(city => city.CityId == hotel.CityId).Select(city => city.State).FirstOrDefault(),
            }).ToList();
            return hotels;
        }

        public HotelDto AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();
            var result = new HotelDto
            {
                HotelId = hotel.HotelId,
                CityId = hotel.CityId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityName = _context.Cities.Where(city => city.CityId == hotel.CityId).Select(city => city.Name).FirstOrDefault(),
                State = _context.Cities.Where(city => city.CityId == hotel.CityId).Select(city => city.State).FirstOrDefault(),
            };
            return result;

        }
    }
}