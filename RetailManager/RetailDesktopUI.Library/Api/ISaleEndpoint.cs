using RetailDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace RetailDesktopUI.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}