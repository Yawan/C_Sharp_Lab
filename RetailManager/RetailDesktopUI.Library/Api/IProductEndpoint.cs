using RetailDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailDesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}