using CountriesServers.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountriesServers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // POST: api/Users/Register
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User user)
        {
            int result = user.Register();

            if (result > 0)
            {
                return Ok(result);
            }

            if (result == -1)
            {
                return Conflict("Email already exists");
            }

            return BadRequest("Registration failed");
        }

        // POST: api/Users/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] User user)
        {
            User? loggedUser = user.Login();

            if (loggedUser == null)
            {
                return Unauthorized("Wrong email or password");
            }

            if (loggedUser.IsLocked)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    "User is locked"
                );
            }

            return Ok(loggedUser);
        }

        // POST: api/Users/Logout
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            return Ok(true);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            User user = new User();

            User? selectedUser = user.GetById(id);

            if (selectedUser == null)
            {
                return NotFound("User was not found");
            }

            return Ok(selectedUser);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult UpdateProfile(
            int id,
            [FromBody] User user)
        {
            user.UserId = id;

            int result = user.UpdateProfile();

            if (result == 1)
            {
                return Ok(result);
            }

            if (result == -1)
            {
                return Conflict("Email already exists");
            }

            return NotFound("User was not found");
        }

        // PUT: api/Users/5/Password
        [HttpPut("{id}/Password")]
        public IActionResult ChangePassword(
            int id,
            [FromBody] ChangePasswordRequest request)
        {
            User user = new User();

            user.UserId = id;
            user.Email = request.Email;

            bool result = user.ChangePassword(
                request.CurrentPassword,
                request.NewPassword
            );

            if (result)
            {
                return Ok(true);
            }

            return BadRequest("Password update failed");
        }

        // GET: api/Users/5/Continents
        [HttpGet("{id}/Continents")]
        public IActionResult GetContinents(int id)
        {
            User user = new User();
            user.UserId = id;

            return Ok(user.GetContinents());
        }

        // POST: api/Users/5/Continents
        [HttpPost("{id}/Continents")]
        public IActionResult AddContinent(
            int id,
            [FromBody] ContinentRequest request)
        {
            User user = new User();
            user.UserId = id;

            int result =
                user.AddContinent(request.ContinentName);

            if (result > 0)
            {
                return Ok(result);
            }

            if (result == -1)
            {
                return Conflict("Continent already exists");
            }

            return BadRequest("Could not add continent");
        }

        // DELETE: api/Users/5/Continents/Europe
        [HttpDelete("{id}/Continents/{continentName}")]
        public IActionResult RemoveContinent(
            int id,
            string continentName)
        {
            User user = new User();
            user.UserId = id;

            bool result =
                user.RemoveContinent(continentName);

            if (result)
            {
                return Ok(true);
            }

            return NotFound("Continent was not found");
        }

        // GET: api/Users/5/Languages
        [HttpGet("{id}/Languages")]
        public IActionResult GetLanguages(int id)
        {
            User user = new User();
            user.UserId = id;

            return Ok(user.GetLanguages());
        }

        // POST: api/Users/5/Languages
        [HttpPost("{id}/Languages")]
        public IActionResult AddLanguage(
            int id,
            [FromBody] UserLanguage language)
        {
            User user = new User();
            user.UserId = id;

            int result = user.AddLanguage(language);

            if (result > 0)
            {
                return Ok(result);
            }

            if (result == -1)
            {
                return Conflict("Language already exists");
            }

            return BadRequest("Could not add language");
        }

        // PUT: api/Users/5/Languages/en
        [HttpPut("{id}/Languages/{languageCode}")]
        public IActionResult UpdateLanguage(
            int id,
            string languageCode,
            [FromBody] ProficiencyRequest request)
        {
            User user = new User();
            user.UserId = id;

            bool result = user.UpdateLanguage(
                languageCode,
                request.ProficiencyLevel
            );

            if (result)
            {
                return Ok(true);
            }

            return NotFound("Language was not found");
        }

        // DELETE: api/Users/5/Languages/en
        [HttpDelete("{id}/Languages/{languageCode}")]
        public IActionResult RemoveLanguage(
            int id,
            string languageCode)
        {
            User user = new User();
            user.UserId = id;

            bool result =
                user.RemoveLanguage(languageCode);

            if (result)
            {
                return Ok(true);
            }

            return NotFound("Language was not found");
        }
    }
}
