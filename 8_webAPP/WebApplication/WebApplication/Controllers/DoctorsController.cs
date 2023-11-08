using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DTO.Requests;
using WebApplication.Repositories.Interfaces;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/doctor")]
    public class DoctorsController : ControllerBase
    {

        private readonly IDoctorDbRepository _doctorDbRepository;

        public DoctorsController(IDoctorDbRepository doctorDbRepository)
        {
            _doctorDbRepository = doctorDbRepository;
        }
        
        [HttpGet]
        [Authorize]   // [Authorize("admin")]
        public async Task<IActionResult> GetDoctorsList()
        {
            var result = await _doctorDbRepository.GetDoctors();
          
            if (result == null)
                return NotFound("There are no doctors");
            
            return Ok(result);
        }
      
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddDoctor([FromBody] DoctorRequestDTO doctor)
        {
            var status = await _doctorDbRepository.PostDoctor(doctor);
            return StatusCode(status.Code, status.Message);
        }
        
        [HttpPut("{idDoctor}")]
        [Authorize]
        public async Task<IActionResult> ModifyDoctor([FromBody] DoctorRequestDTO doctor, [FromRoute] int idDoctor)
        {
            var status = await _doctorDbRepository.PutDoctor(doctor, idDoctor);
            return StatusCode(status.Code, status.Message);
        }
        
        [HttpDelete("{idDoctor}")]
        [Authorize]
        public async Task<IActionResult> DeleteDoctor([FromRoute] int idDoctor)
        {
            var status = await _doctorDbRepository.DeleteDoctor(idDoctor);
            return StatusCode(status.Code, status.Message);
        }
        
        
    }
}