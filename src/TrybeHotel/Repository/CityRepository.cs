using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<CityDto> GetCities()
        {
            var results = _context.Cities.Select(city => new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State
            }).ToList();
            return results;
        }

        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();
            return new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State
            };
        }

        public CityDto UpdateCity(City city)
        {            
            var updateCity = _context.Cities.Find(city.CityId);
            if (updateCity == null) {
                return null!;
            }
            updateCity.Name = city.Name;
            updateCity.State = city.State;
            _context.SaveChanges();

            var newCity = new CityDto {
                Name = updateCity.Name,
                State = updateCity.State,
                CityId = updateCity.CityId,
            };

            return newCity;
        }

    }
}