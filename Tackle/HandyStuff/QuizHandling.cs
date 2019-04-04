using Networking;
using Newtonsoft.Json;

namespace Quiz
{
    public class QuizHandling
    {
        public static string GetQuizJSON(int quizID)
        {
            ServerConnection server = new ServerConnection();
            string JSON = server.ServerRequest("OPENQUIZ", new string[] { quizID.ToString() });
            return JSON;
        }

        // Gets the quiz information for students

        public static (string[], int[], string[], int, string) OpenQuiz(string JSON)
        {
            Quiz quiz = JsonConvert.DeserializeObject<Quiz>(JSON);

            return (quiz.Questions, quiz.QuestionTypes, quiz.Answers, quiz.TimeAllocated, quiz.QuizType);
        }

        // Used in the marking of quizzes to get the information from the QuizAttempts table

        public static (int, string, string, string[], string[], bool[], bool) GetQuizInformation(string username, int quizID)
        {
            QuizInstance quizInstance = new QuizInstance();

            var serverConnection = new ServerConnection();
            string serverResponse = serverConnection.ServerRequest("QUIZMARKINGVIEW", new string[2] {username,quizID.ToString()});
            serverResponse = serverResponse.Replace("\0", string.Empty);

            quizInstance = DeserialiseQuizAttempt(serverResponse, quizInstance);
            
            if (serverResponse != "FALSE")
            {
                return (quizInstance.quizID, quizInstance.username, quizInstance.quizType, quizInstance.questions, quizInstance.answers, quizInstance.correct,true);
            }
            else
            {
                return (quizInstance.quizID, quizInstance.username, quizInstance.quizType, quizInstance.questions, quizInstance.answers, quizInstance.correct, false);
            }
        }

        static QuizInstance DeserialiseQuizAttempt(string JSON, QuizInstance quizInstance)
        {
            quizInstance = JsonConvert.DeserializeObject<QuizInstance>(JSON);
            return quizInstance;
        }

    }
    //Used temporarily to store the values when deserialising the quiz attempt
    public class QuizInstance
    {
        public int quizID { get; set; }
        public string username { get; set; }
        public string quizType { get; set; }
        public string[] questions { get; set; }
        public string[] answers { get; set; }
        public int correctTotal { get; set; }
        public bool[] correct { get; set; }
    }
}
