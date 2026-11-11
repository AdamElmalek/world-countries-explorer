using CountriesServers.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountriesServers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        // GET: api/<AdminController>
        [HttpGet("{adminId}/Users")]
        public IActionResult GetUsers(int adminId)
        {
            Admin admin = new Admin();
            admin.AdminId = adminId;

            if (!admin.IsAdmin())
            {
                return StatusCode(403, "Admin permission is required");
            }

            return Ok(admin.GetUsers());
        }

        [HttpPut("{adminId}/Users/{userId}/Lock")]
        public IActionResult SetUserLocked(
             int adminId,
             int userId,
             [FromBody] AdminBooleanRequest request)
        {
            Admin admin = new Admin();
            admin.AdminId = adminId;

            if (!admin.IsAdmin())
            {
                return StatusCode(403, "Admin permission is required");
            }

            bool result = admin.SetUserLocked(userId, request.Value);

            if (result)
            {
                return Ok(true);
            }

            return NotFound("User was not found");
        }

        [HttpPut("{adminId}/Users/{userId}/Share")]
        public IActionResult SetUserCanShare(
            int adminId,
            int userId,
            [FromBody] AdminBooleanRequest request)
        {
            Admin admin = new Admin();
            admin.AdminId = adminId;

            if (!admin.IsAdmin())
            {
                return StatusCode(403, "Admin permission is required");
            }

            bool result = admin.SetUserCanShare(userId, request.Value);

            if (result)
            {
                return Ok(true);
            }

            return NotFound("User was not found");
        }

        [HttpDelete("{adminId}/Shares/{shareId}")]
        public IActionResult DeleteShare(int adminId, int shareId)
        {
            Admin admin = new Admin();
            admin.AdminId = adminId;

            if (!admin.IsAdmin())
            {
                return StatusCode(403, "Admin permission is required");
            }

            bool result = admin.DeleteShare(shareId);

            if (result)
            {
                return Ok(true);
            }

            return NotFound("Share was not found");
        }

        [HttpGet("{adminId}/Statistics")]
        public IActionResult GetStatistics(int adminId)
        {
            Admin admin = new Admin();
            admin.AdminId = adminId;

            if (!admin.IsAdmin())
            {
                return StatusCode(403, "Admin permission is required");
            }

            return Ok(admin.GetStatistics());
        }
    }
}
