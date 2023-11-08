using System;
using System.Threading.Tasks;
using WebApplication.DTO.Requests;
using WebApplication.DTO.Responses;
using WebApplication.Status;

namespace WebApplication.Repositories.Interfaces
{
    public interface IAccountDbRepository
    {
        Task<MyStatus> Register(RegisterRequestDTO register);
        Task<AccountResponseDTO> ValidateLogin(LoginRequestDTO accountData);
        Task<AccountResponseDTO> ValidateRefreshToken(Guid refreshToken, DateTime timer);
        Task AddRefreshToken(string login, Guid refreshToken, DateTime time);
    }
}