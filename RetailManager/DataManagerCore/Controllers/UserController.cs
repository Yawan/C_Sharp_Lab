using DataManager.Library.DataAccess;
using DataManager.Library.Models;
using DataManagerCore.Data;
using DataManagerCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DataManagerCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public UserModel GetById()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById(userId).First();
        }

        [Authorize(Roles = "Admin")]
        // [AllowAnonymous] // swagger API test only
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        [ProducesResponseType(typeof(List<ApplicationUserModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ApplicationUserModel>>> GetAllUsers()
        {
            try
            {
                // 使用單一查詢優化性能
                var users = await _context.Users
                    .AsNoTracking()  // 提高性能，因為我們只是讀取數據
                    .Select(user => new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Roles = _context.UserRoles
                            .Where(ur => ur.UserId == user.Id)
                            .Join(
                                _context.Roles,
                                ur => ur.RoleId,
                                r => r.Id,
                                (ur, r) => new { ur.RoleId, r.Name }
                            )
                            .ToDictionary(x => x.RoleId, x => x.Name)
                    })
                    .ToListAsync();

                //_logger.LogInformation("Successfully retrieved {Count} users", users.Count);
                return Ok(users);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error occurred while retrieving users");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while processing your request." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            var roleDictionary = _context.Roles.ToDictionary(role => role.Id, role => role.Name);
            return roleDictionary;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/AddRole")]
        public async Task AddRole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.AddToRoleAsync(user, pairing.RoleName);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/RemoveRole")]
        public async Task RemoveRole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);
        }
    }
}