using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Requests;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        private readonly ITripDbRepository _tripDbRepository;

        public TripsController(ITripDbRepository tripDbRepository)
        {
            _tripDbRepository = tripDbRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var tripsFromDb = await _tripDbRepository.GetTripsFromDb();

            return Ok(tripsFromDb);

        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClient([FromRoute] int idTrip, [FromBody] ClientRequestDTO client)
        { 
            var status = await _tripDbRepository.PostClientToTrip(idTrip, client);
            return StatusCode(status.Code, status.Message);
        }
        

    }
}