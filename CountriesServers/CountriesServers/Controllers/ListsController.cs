using CountriesServers.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountriesServers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        [HttpPost]
        public IActionResult AddCountry([FromBody] UserCountryList userCountry)
        {
            int result = userCountry.Add();

            if (result > 0)
            {
                return Ok(result);
            }

            if (result == -1)
            {
                return Conflict("Country is already saved");
            }

            if (result == -2)
            {
                return NotFound("User was not found");
            }

            if (result == -3)
            {
                return NotFound("Country was not found");
            }

            return BadRequest("List information is invalid");
        }

        // Get all countries from one personal list.
        [HttpGet("{userId:int}/{listType}")]
        public IActionResult GetByType(int userId, string listType)
        {
            UserCountryList userCountry = new UserCountryList();
            List<UserCountryList> countries = userCountry.GetByType(userId, listType);

            return Ok(countries);
        }

        // Check whether one country is saved.
        [HttpGet("{userId:int}/{countryId:int}/Status")]
        public IActionResult GetStatus(int userId, int countryId)
        {
            UserCountryList userCountry = new UserCountryList();
            userCountry.UserId = userId;
            userCountry.CountryId = countryId;

            UserCountryList? status = userCountry.GetStatus();

            if (status == null)
            {
                return NotFound("Country is not saved");
            }

            return Ok(status);
        }

        // Move a country between Visited and WantToVisit.
        [HttpPut("{userId:int}/{countryId:int}")]
        public IActionResult MoveCountry(int userId, int countryId, [FromBody] UserCountryList userCountry)
        {
            userCountry.UserId = userId;
            userCountry.CountryId = countryId;

            bool result = userCountry.Move(userCountry.ListType);

            if (result)
            {
                return Ok(true);
            }

            return BadRequest("Country was not moved");
        }

        // Remove a country from the personal lists.
        [HttpDelete("{userId:int}/{countryId:int}")]
        public IActionResult RemoveCountry(int userId, int countryId)
        {
            UserCountryList userCountry = new UserCountryList();
            userCountry.UserId = userId;
            userCountry.CountryId = countryId;

            bool result = userCountry.Remove();

            if (result)
            {
                return Ok(true);
            }

            return NotFound("Country was not saved");
        }
    }
}
