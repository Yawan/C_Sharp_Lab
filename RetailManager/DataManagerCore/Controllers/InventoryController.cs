using DataManager.Library.DataAccess;
using DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataManagerCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IInventoryData _inventoryData;

        public InventoryController(IConfiguration config, IInventoryData inventoryData)
        {
            _config = config;
            _inventoryData = inventoryData;
        }

        [Authorize(Roles = "Admin,Manager")] // OR
        [HttpGet]
        public List<InventoryModel> Get()
        {
            return _inventoryData.GetInventory();
        }

        //[Authorize(Roles = "WarehouseWorker")] AND
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post(InventoryModel item)
        {
            _inventoryData.SaveInventoryRecord(item);
        }
    }
}