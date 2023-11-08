using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;
using RestApi.Services;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/warehouses")]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseDbService _warehouseDbService;
        public WarehousesController(IWarehouseDbService warehouseDbService)
        {
            _warehouseDbService = warehouseDbService;
        }

        [HttpPost]
        public async Task<IActionResult> PutProductIntoWarehouse([FromBody] ProductWarehouse productWarehouse)
        {
            var answer = await _warehouseDbService.ReturnAnswer(productWarehouse);
            
            return StatusCode(answer.Code, answer.Message);
        }
        
    }
}