using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplication.DTO.Requests;
using WebApplication.DTO.Responses;
using WebApplication.Repositories.Interfaces;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountDbRepository _accountsDbRepository;
        
        public AccountsController(IConfiguration configuration, IAccountDbRepository accountsDbRepository)
        {
            _configuration = configuration;
            _accountsDbRepository = accountsDbRepository;
        }
        
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO register)
        {
           // throw new Exception("test");
           var answer = await _accountsDbRepository.Register(register);
            return StatusCode(answer.Code, answer.Message);
        }
        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO accountData)
        {
            // z body przyjmujemy login i haslo
            // sprawdzamy w bazie czy uzytkownik istnieje
            // jak nie 400 bad request
            // jak tak to zwracamy uzytkownika do kontrolera
            // var userFromDb = 

            var result = await _accountsDbRepository.ValidateLogin(accountData);
          
            if (result == null)
                return NotFound("Wrong login, try again");
            
            var time = DateTime.Now.AddMinutes(10);
            var rToken = Guid.NewGuid();

            // aby dodac/zmienic refresh token pobieram login, refreshToken i nowy czas
            await _accountsDbRepository.AddRefreshToken(accountData.Login, rToken, time); 
            return Ok(new
            {
                appToken = new JwtSecurityTokenHandler().WriteToken(GenerateToken(result, time)),
                refreshToken = rToken
            });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody]Guid refreshToken)
        {
            // Guida sie wpisuje jak string (w nawiasach) -> wpisywanie w swagger 
            
            // przyjmij refresh token z ciala zadania, sprawdz czy w bazie jest zapisany ten token przy ktoryms z uzytkownikow,
            // jak nie ma lub wygasl to zwroc 400 bad request
            // jak jest i nie wygasl to pobiez uzytkownika z bazy i wygeneruj mu nowy token oraz refresh token (powinien byc znowu zapisany do bazy z nowym czasem)
            // var userFromDb = 
            
            var result = await _accountsDbRepository.ValidateRefreshToken(refreshToken, DateTime.Now);
          
            if (result == null)
                return NotFound("Wrong token, try again");
           
            var time = DateTime.Now.AddMinutes(10);
            var rToken = Guid.NewGuid();

            await _accountsDbRepository.AddRefreshToken(result.Login, rToken, time);
            return Ok(new
            {
                appToken = new JwtSecurityTokenHandler().WriteToken(GenerateToken(result, time)),
                refreshToken = rToken
            });
        }
        
        private JwtSecurityToken GenerateToken(AccountResponseDTO accountData, DateTime time)
        {
            // wartosci pobieramy z userFromDb  z bazy danych !!!, pobieram tylko login
            Claim[] userClaims =
            {
                // new Claim(ClaimTypes.NameIdentifier, accountData.IdLogin),
                new Claim(ClaimTypes.Name, accountData.Login),
                // new Claim(ClaimTypes.Role, "admin"),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: userClaims,
                expires: time,
                signingCredentials: creds
            );

            return token;
        }

        
    }
}