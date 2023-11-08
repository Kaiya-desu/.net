using System.Threading.Tasks;

namespace WebApplication1.Repositories
{
    public interface IClientDbRepository
    {
        Task<MyStatus> DeleteClientFromDb(int idClient);
    }
}