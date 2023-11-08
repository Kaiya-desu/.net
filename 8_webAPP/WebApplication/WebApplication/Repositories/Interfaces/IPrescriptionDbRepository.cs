using System.Threading.Tasks;
using WebApplication.DTO.Responses;

namespace WebApplication.Repositories.Interfaces
{
    public interface IPrescriptionDbRepository
    {
        Task <PrescriptionResponseDTO> GetPrescription(int idPrescription);
    }
}