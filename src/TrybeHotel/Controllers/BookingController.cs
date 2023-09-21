using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]

    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(Policy = "client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            // throw new NotImplementedException();
            var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;
            var newBook = _repository.Add(bookingInsert, userEmail);

            if (newBook == null)
            {
                return BadRequest(new
                {
                    message = "Guest quantity over room capacity"
                });
            }

            return Created("", newBook);
        }


        [HttpGet("{Bookingid}")]
        [Authorize(Policy = "client")]
        public IActionResult GetBooking(int Bookingid)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var booking = _repository.GetBooking(Bookingid, userEmail!);
            if (booking == null)
                return Unauthorized(new { message = "Booking not found" });

            return Ok(booking);
        }
    }
}