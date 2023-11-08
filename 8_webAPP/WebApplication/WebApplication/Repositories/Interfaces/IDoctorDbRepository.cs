using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.DTO.Requests;
using WebApplication.DTO.Responses;
using WebApplication.Status;

namespace WebApplication.Repositories.Interfaces
{
    public interface IDoctorDbRepository
    {
        Task <ICollection<DoctorResponseDTO>> GetDoctors();
        Task<MyStatus> PostDoctor(DoctorRequestDTO doctor);
        Task<MyStatus> PutDoctor(DoctorRequestDTO doctor, int idDoctor);
        Task<MyStatus> DeleteDoctor(int idDoctor);

    }
}