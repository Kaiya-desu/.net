using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories.Implementations
{
    public class ClientDbRepository : IClientDbRepository
    {
        private readonly _2019SBDContext _context;

        public ClientDbRepository(_2019SBDContext context)
        {
        _context = context;
        }
    
        public async Task<MyStatus> DeleteClientFromDb(int idClient)
        {
            var code = 0;
            var message = "";

            var clientDb = await _context.Clients.SingleOrDefaultAsync(x => x.IdClient == idClient);
            var clientsTrip = await _context.ClientTrips.AnyAsync(x => x.IdClient == idClient);  // bo jeden klient moze byc na kilku wycieczkach
            if (clientDb == null)
            {
                code = 404;
                message = "Wrong IdClient, Can't delete!";
            }
            // przypisana wycieczka do klienta
            else if (clientsTrip != null)
            {
                code = 404;
                message = "Client is on trip, Can't delete!";
            }
            else
                _context.Remove(clientDb);

            if (await _context.SaveChangesAsync() > 0)
            {
                code = 200;
                message = "Deleted!";
            }

            var answer = new MyStatus
            {
                Code = code,
                Message = message
            };

            return answer;
        }
    }
}