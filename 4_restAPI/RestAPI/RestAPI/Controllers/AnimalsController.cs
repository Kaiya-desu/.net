using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Services;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalDbService _animalDbService;
        
        public AnimalController(IAnimalDbService animalDbService)
        {
            _animalDbService = animalDbService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAnimals([FromQuery] string orderBy)
        {
            
            var result = _animalDbService.GetAnimalsFromDb(orderBy);
            if (result == null)
                return NotFound("Database is empty.");
            
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] Animal animal)
        {
            if(!_animalDbService.ValidateData(animal))
                return BadRequest("Incorrect values. Try again");
            
            _animalDbService.AddAnimalToDb(animal);
            return Created("Created ", "Created " + animal);
        }
        
        [HttpPut("{IdAnimal}")]
        public async Task<IActionResult> ModifyAnimal([FromRoute] string idAnimal, [FromBody] Animal animal)
        {
            if (idAnimal is null)
                return BadRequest("Can't be null");
            if (!_animalDbService.ValidateId(idAnimal))
                return NotFound("There is no " + idAnimal + " in the database");
            if (!_animalDbService.ValidateData(animal))
                return BadRequest("Incorrect values. Try again");
            
            _animalDbService.ModifyAnimalInDb(idAnimal, animal);
            return Created("Modified ", "Modified " + animal);
        }
        
        [HttpDelete("{IdAnimal}")]
        public async Task<IActionResult> DeleteAnimal([FromRoute] string idAnimal)
        {
            if (idAnimal is null)
                return BadRequest("Can't be null");
            if (!_animalDbService.ValidateId(idAnimal))
                return NotFound("There is no " + idAnimal + " in the database");
            
            _animalDbService.DeleteAnimalFromDb(idAnimal);
            return Ok("Deleted animal " + idAnimal);
        }
    }
}