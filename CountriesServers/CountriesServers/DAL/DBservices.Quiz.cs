using CountriesServers.Models;
using System.Data;
using System.Data.SqlClient;

namespace CountriesServers.DAL
{
    public partial class DBservices
    {
        //SP_Quizzes_GetActive
        public List<Quiz> GetActiveQuizzes()
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<Quiz> quizzes = new List<Quiz>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();

                cmd = CreateCommandWithStoredProcedureGeneral("SP_Quizzes_GetActive", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Quiz quiz = new Quiz();

                    quiz.QuizId = Convert.ToInt32(dataReader["QuizId"]);
                    quiz.Title = ReadString(dataReader, "Title");
                    quiz.Description = ReadString(dataReader, "Description");
                    quiz.TimeLimitSeconds = Convert.ToInt32(dataReader["TimeLimitSeconds"]);
                    quiz.IsActive = Convert.ToBoolean(dataReader["IsActive"]);
                    quiz.CreatedAt = Convert.ToDateTime(dataReader["CreatedAt"]);
                    quiz.QuestionsCount = Convert.ToInt32(dataReader["QuestionsCount"]);
                    quiz.MaximumScore = Convert.ToInt32(dataReader["MaximumScore"]);

                    quizzes.Add(quiz);

                }

                return quizzes;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        //SP_QuizQuestions_GetByQuiz
        public List<QuizQuestion> GetQuizQuestions(int quizId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<QuizQuestion> questions = new List<QuizQuestion>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@QuizId", quizId);
                cmd = CreateCommandWithStoredProcedureGeneral("SP_QuizQuestions_GetByQuiz", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    QuizQuestion question = new QuizQuestion();

                    question.QuestionId = Convert.ToInt32(dataReader["QuestionId"]);
                    question.QuizId = Convert.ToInt32(dataReader["QuizId"]);
                    question.QuestionText = ReadString(dataReader, "QuestionText");
                    question.OptionA = ReadString(dataReader, "OptionA");
                    question.OptionB = ReadString(dataReader, "OptionB");
                    question.OptionC = ReadString(dataReader, "OptionC");
                    question.OptionD = ReadString(dataReader, "OptionD");
                    question.Points = Convert.ToInt32(dataReader["Points"]);

                    questions.Add(question);

                }

                return questions;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        // Get correct answers for server-side score calculation :SP_QuizQuestions_GetCorrectAnswers
        public List<QuizCorrectAnswer> GetQuizCorrectAnswers(int quizId)
        {

            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<QuizCorrectAnswer> answers = new List<QuizCorrectAnswer>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@QuizId", quizId);
                cmd = CreateCommandWithStoredProcedureGeneral("SP_QuizQuestions_GetCorrectAnswers", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    QuizCorrectAnswer answer = new QuizCorrectAnswer();

                    answer.QuestionId = Convert.ToInt32(dataReader["QuestionId"]);
                    answer.CorrectOption = ReadString(dataReader, "CorrectOption");
                    answer.Points = Convert.ToInt32(dataReader["Points"]);

                    answers.Add(answer);

                }

                return answers;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        // Start a new quiz attempt. :SP_UserQuizAttempts_Start
        public int StartQuizAttempt(int userId, int quizId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", userId);
                paramDic.Add("@QuizId", quizId);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserQuizAttempts_Start", con, paramDic);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        // Save the completed quiz result. :SP_UserQuizAttempts_Complete
        public int CompleteQuizAttempt(int attemptId, int userId, int quizId, int score, int correctAnswers, int totalQuestions)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@AttemptId", attemptId);
                paramDic.Add("@UserId", userId);
                paramDic.Add("@QuizId", quizId);
                paramDic.Add("@Score", score);
                paramDic.Add("@CorrectAnswers", correctAnswers);
                paramDic.Add("@TotalQuestions", totalQuestions);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserQuizAttempts_Complete", con, paramDic);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        // Get all quiz attempts made by one user.:SP_UserQuizAttempts_GetByUser
        public List<UserQuizAttempt> GetQuizAttemptsByUser(int userId)
        {

            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            List<UserQuizAttempt> attempts = new List<UserQuizAttempt>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", userId);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserQuizAttempts_GetByUser", con, paramDic);

                using SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    UserQuizAttempt attempt = new UserQuizAttempt();

                    attempt.AttemptId = Convert.ToInt32(dataReader["AttemptId"]);
                    attempt.UserId = Convert.ToInt32(dataReader["UserId"]);
                    attempt.QuizId = Convert.ToInt32(dataReader["QuizId"]);
                    attempt.Title = ReadString(dataReader, "Title");
                    attempt.Score = Convert.ToInt32(dataReader["Score"]);
                    attempt.CorrectAnswers = Convert.ToInt32(dataReader["CorrectAnswers"]);
                    attempt.TotalQuestions = Convert.ToInt32(dataReader["TotalQuestions"]);
                    attempt.StartedAt = Convert.ToDateTime(dataReader["StartedAt"]);
                    attempt.FinishedAt = ReadNullableDateTime(dataReader, "FinishedAt");
                    attempt.IsCompleted = Convert.ToBoolean(dataReader["IsCompleted"]);

                    attempts.Add(attempt);

                }

                return attempts;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        // Get the sum of the user's best score in every quiz.: SP_UserQuizAttempts_GetTotalPoints
        public int GetTotalQuizPoints(int userId)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserId", userId);

                cmd = CreateCommandWithStoredProcedureGeneral("SP_UserQuizAttempts_GetTotalPoints", con, paramDic);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }
    }
}
