using CountriesServers.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountriesServers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharesController : ControllerBase
    {
        // getall shares in website 
        [HttpGet]
        public IActionResult GetAll()
        {
            Share share = new Share();
            return Ok(share.GetAll());
        }

        // get all shares from 1 country
        [HttpGet("ByCountry/{countryId}")]
        public IActionResult GetByCountry(int countryId)
        {
            Share share = new Share();
            return Ok(share.GetByCountry(countryId));
        }

        // add new share
        [HttpPost]
        public IActionResult Add([FromBody] Share share)
        {
            int result = share.Add();

            if (result > 0)
            {
                return Ok(result);
            }

            if (result == -1)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    "The user is blocked from sharing content");

            }

            return BadRequest("Share information is invalid");

        }

        // update a share belong to the user
        [HttpPut("{shareId}")]
        public IActionResult Update(int shareId, [FromBody] Share share)
        {
            share.ShareId = shareId;
            int result = share.Update();

            if (result == 1)
            {
                return Ok(result);
            }

            if (result == -1)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    "The user is blocked from sharing content");
            }

            return NotFound("Share was not found or does not belong to this user");
        }

        // delete a share belong to the user 
        [HttpDelete("{shareId}")]
        public IActionResult Delete(int shareId, int userId)
        {
            Share share = new Share();
            share.ShareId = shareId;
            share.UserId = userId;

            bool result = share.Delete();

            if (result)
            {
                return Ok(true);
            }

            return NotFound("Share was not found or does not belong to this user");
        }
    }
}
