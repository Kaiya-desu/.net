using System.Threading.Tasks;
using RestApi.Models;

namespace RestApi.Services
{
    public interface IWarehouseDbService
    {
        public Task<MyStatus> ReturnAnswer(ProductWarehouse productWarehouse);
    }
    
}