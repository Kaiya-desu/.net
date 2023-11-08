using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Repositories.Interfaces;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/doctor")]
    [Authorize]
    public class PrescriptionsController : ControllerBase
    {
        
        private readonly IPrescriptionDbRepository _prescriptionDbRepository;

        public PrescriptionsController(IPrescriptionDbRepository prescriptionDbRepository)
        {
            _prescriptionDbRepository = prescriptionDbRepository;
        }
        
        // na podstawie danych pacjenta, doktora i lisy lekow na recepte
        [HttpGet("{idPrescription}")]
        [Authorize]
        public async Task<IActionResult> GetPrescription(int idPrescription)
        {
            var result = await _prescriptionDbRepository.GetPrescription(idPrescription);
          
            if (result == null)
                return NotFound("There is no prescription");
            
            return Ok(result);
        }
    }
}