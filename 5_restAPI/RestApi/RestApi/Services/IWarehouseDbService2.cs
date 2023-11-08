using System.Threading.Tasks;
using RestApi.Models;

namespace RestApi.Services
{
    public interface IWarehouseDbService2
    {
        public Task<MyStatus> Transaction(ProductWarehouse productWarehouse);
    }
}