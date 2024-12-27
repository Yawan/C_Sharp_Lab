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
        private readonly IConfiguration _config;
        private readonly ISaleData _saleData;

        public SaleController(IConfiguration config, ISaleData saleData)
        {
            _config = config;
            _saleData = saleData;
        }

        [Authorize(Roles = "Cashier")]
        [HttpPost]
        public void Post([FromBody] SaleModel sale)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// RequestContext.Principal.Identity.GetUserId();

            _saleData.SaveSale(sale, userId);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            return _saleData.GetSaleReport();
        }
    }
}