using CountriesServers.DAL;

namespace CountriesServers.Models
{
    public class QuizSubmission
    {
        public int AttemptId { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public List<QuizAnswer> Answers { get; set; }

        public QuizSubmission()
        {
            Answers = new List<QuizAnswer>();
        }
        public QuizResult? Submit()
        {
            if (AttemptId <= 0 || UserId <= 0 || QuizId <= 0)
            {
                return null;
            }

            DBservices dbs = new DBservices();
            List<QuizCorrectAnswer> correctAnswers = dbs.GetQuizCorrectAnswers(QuizId);

            if (correctAnswers.Count == 0)
            {
                return null;
            }

            int score = 0;
            int correctCount = 0;

            for (int i = 0; i < correctAnswers.Count; i++)
            {
                QuizCorrectAnswer correctAnswer = correctAnswers[i];

                for (int j = 0; j < Answers.Count; j++)
                {
                    QuizAnswer submittedAnswer = Answers[j];

                    if (submittedAnswer.QuestionId == correctAnswer.QuestionId)
                    {
                        string selectedOption = submittedAnswer.SelectedOption.Trim().ToUpper();

                        if (selectedOption == correctAnswer.CorrectOption.ToUpper())
                        {
                            score += correctAnswer.Points;
                            correctCount++;
                        }

                        break;
                    }
                }
            }

            int result = dbs.CompleteQuizAttempt(
                AttemptId,
                UserId,
                QuizId,
                score,
                correctCount,
                correctAnswers.Count
            );

            if (result == 0)
            {
                return null;
            }

            QuizResult quizResult = new QuizResult();

            quizResult.AttemptId = AttemptId;
            quizResult.TotalQuestions = correctAnswers.Count;
            quizResult.TimeExpired = result == -1;

            if (result == -1)
            {
                quizResult.Score = 0;
                quizResult.CorrectAnswers = 0;
            }
            else
            {
                quizResult.Score = score;
                quizResult.CorrectAnswers = correctCount;
            }

            quizResult.TotalPoints = dbs.GetTotalQuizPoints(UserId);

            return quizResult;
        }
    }
}
