using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication.DTO.Requests;
using WebApplication.DTO.Responses;
using WebApplication.Models;
using WebApplication.Repositories.Interfaces;
using WebApplication.Status;

namespace WebApplication.Repositories.Implementations
{
    public class AccountDbRepository : IAccountDbRepository
    {
        private readonly HospitalContext _context;

        public AccountDbRepository(HospitalContext context)
        {
            _context = context;
        }

        public async Task<MyStatus> Register(RegisterRequestDTO register)
        {
            var code = 0;
            var message = "";
            
            var validateLogin = await _context.Account.SingleOrDefaultAsync(d => d.Login == register.Login);
            
            if (validateLogin != null)
            {
                code = 404;
                message = "Login taken!";
            }
            else
            {
                // SALT
                var hashedPassword = new PasswordHasher<Account>().HashPassword(new Account(), register.Password);
                await _context.AddAsync(new Account
                {
                    Login = register.Login,
                    Password = hashedPassword,
                    Email = register.Email
                });

                await _context.SaveChangesAsync();

                var newUserId = _context.Account.Max(d => d.IdAccount);
                code = 200;
                message = "New account registered, ID = " + newUserId;
            }
            
            var answer = new MyStatus
            {
                Code = code,
                Message = message
            };

            return answer;
        }
        
        public async Task<AccountResponseDTO> ValidateLogin(LoginRequestDTO accountData)
        {
            // znajdz haslo dla danego konta
            var getPasswordFromDb = await _context.Account
                .Where(account => account.Login == accountData.Login)
                .Select(account => account.Password).SingleOrDefaultAsync();
            
            // czy haslo sie zgadza?
            var verifyPassword = new PasswordHasher<Account>()
                .VerifyHashedPassword(new Account(), getPasswordFromDb,accountData.Password);
            
            if (verifyPassword == PasswordVerificationResult.Success)
            {
                var accountFromDb = await _context.Account
                    .Where(account => account.Login == accountData.Login )
                    .Select(account => new AccountResponseDTO
                    {
                        Login = account.Login
                    }).SingleOrDefaultAsync();

                return accountFromDb;
            }
            
            return null;
        }

        public async Task AddRefreshToken(string login, Guid refreshToken, DateTime time)
        {
            var findLogin = await _context.Account.SingleOrDefaultAsync(a => a.Login == login);
            if (findLogin != null)
            {
                findLogin.RefreshToken = refreshToken;
                findLogin.TokenExpires = time;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<AccountResponseDTO> ValidateRefreshToken(Guid refreshToken, DateTime timer)
        {
            var accountFromDb = await _context.Account
                .Where(account => account.RefreshToken == refreshToken && account.TokenExpires >= timer)
                .Select(account => new AccountResponseDTO
                {
                    Login = account.Login
                }).SingleOrDefaultAsync();

            return accountFromDb;
        }
        
        
    }
}