using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Networking;
using Newtonsoft.Json;

namespace Quiz
{
    public class QuizHandling
    {
        private static void ExtractZip(int quizID)
        {
            using (ZipArchive quizZip = ZipFile.Open($"{quizID}.zip", ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry file in quizZip.Entries)
                {
                    if (file.Name == "questions.txt")
                    {
                        file.ExtractToFile("questions.txt");
                    }
                    else if (file.Name == "metadata.txt")
                    {
                        file.ExtractToFile("metadata.txt");
                    }
                    else if (file.Name == "answers.txt")
                    {
                        file.ExtractToFile("answers.txt");
                    }
                    else if (file.Name == "questiontypes.txt")
                    {
                        file.ExtractToFile("questiontypes.txt");
                    }
                }
            }
        }

        public static string[] GetMetadata()
        {
            string[] metadata = File.ReadAllLines("metadata.txt");
            File.Delete("metadata.txt");
            return metadata;
        }

        public static string[] GetQuestions()
        {
            string[] questions = File.ReadAllLines("questions.txt");
            File.Delete("questions.txt");
            return questions;
        }

        public static string[] GetAnswers()
        {
            string[] answers = File.ReadAllLines("answers.txt");
            File.Delete("answers.txt");
            return answers;
        }

        public static string[] GetQuestionTypes()
        {
            string[] answertypes = File.ReadAllLines("questiontypes.txt");
            File.Delete("questiontypes.txt");
            return answertypes;
        }

        // Gets the quiz information for students

        public static (string[], string[], string[], int) OpenQuiz()
        {
            ExtractZip(0);

            string[] metadata = GetMetadata();
            string[] questions = GetQuestions();
            string[] questionTypes = GetQuestionTypes();
            List<string> answers = new List<string>();

            //If the quiz type is instant, then the answers will need to be fetched from the answers.txt file
            if (metadata[1] == "INSTANT")
            {
                string[] answersArray = GetAnswers();
                foreach(string answer in answersArray)
                {
                    answers.Add(answer);
                }
            }
            //TODO: ADD ABILITY FOR NON INSTANT QUIZZES AS WELL, RETURN THE QUIZ TYPE
            return (questions, questionTypes, answers.ToArray<string>(),Int32.Parse(metadata[0]));
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

        //Used temporarily to store the values when deserialising the quiz attempt
        class QuizInstance
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
}
