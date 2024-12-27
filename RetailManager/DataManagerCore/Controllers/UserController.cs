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
using System.Linq.Expressions;
using System.Security.Claims;

namespace DataManagerCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserData _userData;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ILogger<UserController> _logger;

        public UserController(IUserData userData, ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<UserController> logger)
        {
            _userData = userData;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public UserModel GetById()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //RequestContext.Principal.Identity.GetUserId();

            return _userData.GetUserById(userId).First();
        }

        [Authorize(Roles = "Admin")]
        // [AllowAnonymous] // swagger API test only
        [HttpGet]
        [Route("Admin/GetAllUsers")]
        [ProducesResponseType(typeof(List<ApplicationUserModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ApplicationUserModel>>> GetAllUsers()
        {
            try
            {
                List<ApplicationUserModel> output = new List<ApplicationUserModel>();

                var users = _context.Users.ToList();
                var userRoles = from ur in _context.UserRoles
                                join r in _context.Roles on ur.RoleId equals r.Id
                                select new { ur.UserId, ur.RoleId, r.Name };

                foreach (var user in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                    };

                    u.Roles = userRoles.Where(x => x.UserId == user.Id)
                        .ToDictionary(x => x.RoleId, x => x.Name);

                    output.Add(u);
                }

                _logger.LogInformation("Successfully retrieved {Count} users", users.Count);
                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while processing your request." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            var roleDictionary = _context.Roles.ToDictionary(role => role.Id, role => role.Name);
            return roleDictionary;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Admin/AddRole")]
        public async Task AddRole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.AddToRoleAsync(user, pairing.RoleName);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Admin/RemoveRole")]
        public async Task RemoveRole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);
        }
    }
}