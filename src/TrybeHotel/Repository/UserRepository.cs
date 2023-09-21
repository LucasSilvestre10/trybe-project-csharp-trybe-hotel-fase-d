using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var result = _context.Users.FirstOrDefault(user => user.Email == login.Email && user.Password == login.Password);
            if (result == null)
            {
                return null!;
            }

            return new UserDto
            {
                UserId = result.UserId,
                Email = result.Email,
                Name = result.Name,
                UserType = result.UserType,
            };
        }
        public UserDto Add(UserDtoInsert user)
        {
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client",
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return new UserDto
            {
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                UserType = newUser.UserType
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(user => user.Email == userEmail);

            if (user == null)
            {
                return null!;
            }

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            };
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var listUsers = _context.Users
               .Select(user => new UserDto
               {
                   UserId = user.UserId,
                   Name = user.Name,
                   Email = user.Email,
                   UserType = user.UserType
               });
            return listUsers;
        }

    }
}