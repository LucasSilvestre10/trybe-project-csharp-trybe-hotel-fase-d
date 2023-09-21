using Microsoft.AspNetCore.Mvc;


namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("/")]
    public class StatusController : Controller
    {
        [HttpGet]
        public ActionResult GetStatus()
        {
            return Ok(new { message = "online" });
        }
    
    }
}
