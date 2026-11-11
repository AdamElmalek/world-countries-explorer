using CountriesServers.Models;
using CountriesServers.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountriesServers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AIController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("Ask")]
        public async Task<IActionResult> Ask(
            [FromBody] AIQuestionRequest request)
        {
            try
            {
                AIService service = new AIService(configuration);

                AIAnswerResponse response =
                    await service.AskCountryAdvisor(request);

                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(503, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "The AI request could not be completed"
                );
            }
        }
    }
}
