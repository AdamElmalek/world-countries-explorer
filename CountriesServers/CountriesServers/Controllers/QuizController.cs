using CountriesServers.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountriesServers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        // Get all active quizzes.
        [HttpGet]
        public IActionResult GetActiveQuizzes()

        {
            Quiz quiz = new Quiz();
            return Ok(quiz.GetActive());
        }

        // Get quiz questions without correct answers.
        [HttpGet("{quizId}/Questions")]
        public IActionResult GetQuestions(int quizId)
        {
            Quiz quiz = new Quiz();
            quiz.QuizId = quizId;

            List<QuizQuestion> questions = quiz.GetQuestions();

            if (questions.Count == 0)
            {
                return NotFound("Quiz questions were not found");
            }

            return Ok(questions);
        }

        // Start a new quiz attempt.
        [HttpPost("{quizId}/Start")]
        public IActionResult StartAttempt(int quizId, int userId)
        {
            Quiz quiz = new Quiz();
            quiz.QuizId = quizId;

            int attemptId = quiz.StartAttempt(userId);

            if (attemptId > 0)
            {
                return Ok(attemptId);
            }

            return BadRequest("Could not start the quiz");

        }

        // Submit answers and calculate the score on the server.
        [HttpPost("Submit")]
        public IActionResult SubmitQuiz([FromBody] QuizSubmission submission)
        {
            QuizResult? result = submission.Submit();

            if (result == null)
            {
                return BadRequest("Quiz submission is invalid");
            }

            return Ok(result);
        }

        // Get one user's quiz history.
        [HttpGet("User/{userId}/Attempts")]
        public IActionResult GetUserAttempts(int userId)
        {
            UserQuizAttempt attempt = new UserQuizAttempt();
            return Ok(attempt.GetByUser(userId));
        }

        // Get the user's total quiz points.
        [HttpGet("User/{userId}/Points")]
        public IActionResult GetUserPoints(int userId)
        {
            UserQuizAttempt attempt = new UserQuizAttempt();
            return Ok(attempt.GetTotalPoints(userId));
        }

    }
}
