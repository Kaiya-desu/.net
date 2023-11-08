using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [ApiController] // - adnotacje, dzięki temu aplikacja ma dostep do klas których obiekty będziemy zwracali jako odpowiedź
    [Route("api/students")] 
    public class StudentsController : ControllerBase
    {

        private readonly IDbService dbService;
        private static HashSet<Student> studentsSet;

        public StudentsController(IDbService dbService)
        {
            this.dbService = dbService;
        }
        
        // GET : localhost: xxxx/api/students/
        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] string orderBy)
        {
            studentsSet = dbService.Show();
            return Ok(studentsSet);
        }
        // GET : localhost: xxxx/api/students/ska
        [HttpGet("{Ska}")]
        public async Task<IActionResult> GetStudent([FromRoute] string ska)
        {
            studentsSet = dbService.Show();
            Student result = null;

            foreach (var outputResult in studentsSet)
            {
                if (outputResult.Ska == ska)
                {
                    result = outputResult;
                }
            }
            return Ok(result);
        }

        [HttpPut("{Ska}")]
        public async Task<IActionResult> PutStudent([FromRoute] string ska, [FromBody] Student student)
        {
            studentsSet = dbService.Show();
            // HashSetu nie da się edytowac wiec kasuje stary rekord i dodaje nowy
            if (ska is not null)
            {
                var x = studentsSet.Where(w => w.Ska.Contains(ska)).FirstOrDefault();
                if (x != null)
                {
                    studentsSet.Remove(x);
                    dbService.setMap(studentsSet);
                    student.Ska = ska;
                    if (dbService.AddStudent(student))
                    {
                        dbService.setMap(studentsSet);
                        return Created("Edited ",  student); // nie wiem czy lepiej created czy Ok("Edited " + student); 
                    }
                    // jezeli wprowadzone dane będą niekompletne
                        dbService.AddStudent(x); // ponowne dodanie usuniętego studenta x 
                        dbService.setMap(studentsSet);
                        return BadRequest("Incorrect values. Try again");
                }
                return NotFound("There is no " + ska + " student");
            }

            return BadRequest("Cant be null");
        }
        
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] Student student)
        {
            studentsSet = dbService.Show();
            
            if (dbService.AddStudent(student))
            {
                dbService.setMap(studentsSet);
                return Created("Added ", student);
            }

            return BadRequest("Incorrect values. Try again");
        }

        [HttpDelete("{Ska}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] string ska)
        {
            
            studentsSet = dbService.Show();
            
            if (ska is not null)
            {
                var x = studentsSet.Where(w => w.Ska.Contains(ska)).FirstOrDefault();

                if (x is not null)
                {
                    studentsSet.Remove(x);
                    dbService.setMap(studentsSet);
                    return Ok("Student " + ska + " deleted");
                }

                return NotFound("There is no " + ska + " student");
            }
            return BadRequest("Cant be null");
        } 
    }
}