using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

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

        // Uses the tuple type to return multiple values at once

        public static (string[], string[], string[], int) OpenQuiz()
        {
            ExtractZip(0);

            string[] metadata = GetMetadata();
            string[] questions = GetQuestions();
            string[] questionTypes = GetQuestionTypes();
            List<string> answers = new List<string>();

            //IF STATEMENT ASKING WHETHER THE QUIZ IS INSTANT OR NOT
            if (true is true)
            {
                string[] answersArray = GetAnswers();
                foreach(string answer in answersArray)
                {
                    answers.Add(answer);
                }
            }

            return (questions, questionTypes, answers.ToArray<string>(),Int32.Parse(metadata[0]));
        }
    }
}
