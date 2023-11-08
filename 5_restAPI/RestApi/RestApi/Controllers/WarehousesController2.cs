using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;
using RestApi.Services;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/warehouses2")]
    public class WarehousesController2 : ControllerBase
    {
        private readonly IWarehouseDbService2 _warehouseDbService2;
        public WarehousesController2(IWarehouseDbService2 warehouseDbService2)
        {
            _warehouseDbService2 = warehouseDbService2;
        }

        [HttpPost]
        public async Task<IActionResult> PutProductIntoWarehouse([FromBody] ProductWarehouse productWarehouse)
        {
            var answer = await _warehouseDbService2.Transaction(productWarehouse);
            
            return StatusCode(answer.Code, answer.Message);
        }
        
    }
}