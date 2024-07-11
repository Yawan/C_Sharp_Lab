using RetailDesktopUI.Models;
using System.Threading.Tasks;

namespace RetailDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> AuthenticateAsync(string username, string password);
    }
}