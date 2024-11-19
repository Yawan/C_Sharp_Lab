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
        [Authorize(Roles = "Admin,Manager")] // OR
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData();
            return data.GetInventory();
        }

        //[Authorize(Roles = "WarehouseWorker")] AND
        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel item)
        {
            InventoryData data = new InventoryData();
            data.SaveInventoryRecord(item);
        }
    }
}