using RetailDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace RetailDesktopUI.Library.Api
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> AuthenticateAsync(string username, string password);

        Task GetLoggedInUserInfo(string token);
    }
}