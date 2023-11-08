using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Requests;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/client")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientDbRepository _clientDbRepository;

        public ClientsController(IClientDbRepository clientDbRepository)
        {
            _clientDbRepository = clientDbRepository;
        }
        
        [HttpDelete("clients/{idClient}/clients")]
        public async Task<IActionResult> DeleteClient([FromRoute] int idClient)
        {
            var status = await _clientDbRepository.DeleteClientFromDb(idClient);
            return StatusCode(status.Code, status.Message);

        }
    }
}