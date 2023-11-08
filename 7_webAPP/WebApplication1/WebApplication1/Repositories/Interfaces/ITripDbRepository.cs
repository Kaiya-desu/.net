using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DTO.Requests;
using WebApplication1.DTO.Responses;

namespace WebApplication1.Repositories
{
    public interface ITripDbRepository
    {
        Task<ICollection<TripResponseDTO>> GetTripsFromDb();
        Task<MyStatus> PostClientToTrip(int idTrip, ClientRequestDTO client);
    }
}