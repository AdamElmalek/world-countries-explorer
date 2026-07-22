using CountriesServers.Models;
using CountriesServers.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountriesServers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        // Import all countries from countries.dev into SQL Server.
        [HttpPost("Import")]
        public async Task<IActionResult> Import()
        {
            try
            {
                CountriesDevService service = new CountriesDevService();

                int importedCountries = await service.ImportCountries();

                if (importedCountries > 0)
                {
                    return Ok(importedCountries);
                }

                return BadRequest("No countries were imported");
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ex.Message
                );
            }
        }


        // Search, filter and sort countries.
        [HttpGet]
        public IActionResult GetCountries(
            string search = "",
            string region = "",
            string language = "",
            string currency = "",
            long? minPopulation = null,
            long? maxPopulation = null,
            decimal? minArea = null,
            decimal? maxArea = null,
            string sortBy = "name",
            string dir = "asc")
        {
            Country country = new Country();

            List<Country> countries = country.Search(
                search,
                region,
                language,
                currency,
                minPopulation,
                maxPopulation,
                minArea,
                maxArea,
                sortBy,
                dir
            );

            return Ok(countries);
        }


        // Get regions, languages, currencies and numeric limits.
        [HttpGet("FilterOptions")]
        public IActionResult GetFilterOptions()
        {
            Country country = new Country();

            CountryFilterOptions options =
                country.GetFilterOptions();

            return Ok(options);
        }


        // Get one complete country by its ID.
        [HttpGet("{id:int}")]
        public IActionResult GetCountry(int id)
        {
            Country country = new Country();

            Country? selectedCountry =
                country.GetById(id);

            if (selectedCountry == null)
            {
                return NotFound("Country was not found");
            }

            return Ok(selectedCountry);
        }


        // Add a country manually.
        [HttpPost]
        public IActionResult InsertCountry(
            [FromBody] Country country)
        {
            int result = country.Insert();

            if (result > 0)
            {
                return Ok(result);
            }

            if (result == -1)
            {
                return Conflict("Country code already exists");
            }

            return BadRequest("Country information is invalid");
        }


        // Update an existing country.
        [HttpPut("{id:int}")]
        public IActionResult UpdateCountry(
            int id,
            [FromBody] Country country)
        {
            country.CountryId = id;

            int result = country.Update();

            if (result == 1)
            {
                return Ok(result);
            }

            if (result == -1)
            {
                return Conflict("Country code already exists");
            }

            return NotFound(
                "Country was not found or the information is invalid"
            );
        }


        // Delete a country.
        [HttpDelete("{id:int}")]
        public IActionResult DeleteCountry(int id)
        {
            Country country = new Country();
            country.CountryId = id;

            bool result = country.Delete();

            if (result)
            {
                return Ok(true);
            }

            return NotFound("Country was not found");
        }
    }
}
