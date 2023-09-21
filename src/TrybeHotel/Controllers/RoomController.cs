using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("room")]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _repository;
        public RoomController(IRoomRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{HotelId}")]
        public IActionResult GetRoom(int HotelId)
        {
            var result = _repository.GetRooms(HotelId);
            return Ok(result);
        }


        [HttpPost]
        [Authorize(Policy = "admin")]
        public IActionResult PostRoom([FromBody] Room room)
        {
            var result = _repository.AddRoom(room);
            return Created("", result);
        }


        [HttpDelete("{RoomId}")]
        [Authorize(Policy = "admin")]
        public IActionResult Delete(int RoomId)
        {
            _repository.DeleteRoom(RoomId);
            return NoContent();
        }
    }
}