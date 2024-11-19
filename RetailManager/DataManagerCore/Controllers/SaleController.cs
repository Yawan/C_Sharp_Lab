using Azure;
using DataManager.Library.DataAccess;
using DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DataManagerCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        [Authorize(Roles = "Cashier")]
        public void Post([FromBody] SaleModel sale)
        {
            SaleData data = new SaleData();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            SaleData data = new SaleData();
            return data.GetSaleReport();
        }
    }
}