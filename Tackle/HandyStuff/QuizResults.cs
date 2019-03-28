using System.Collections.Generic;

namespace Results
{
    //Holds the information about a student's quiz attempt - includes all of the quiz information for easy displaying
    public class QuizResults
    {
        public int quizID;
        public string username;
        public string quizType;
        public string[] questions;
        public string[] answers;
        //Only used in instant quizzes
        public int correctTotal;
        public List<bool> correct;

        //Makes it so that when a new instance is created, most of the values are already inputted
        public QuizResults(int quizID, string username, string quizType, string[] questions, string[] answers)
        {
            this.quizID = quizID;
            this.username = username;
            this.quizType = quizType;
            this.questions = questions;
            this.answers = answers;

            //Makes the "correct" list not null so it can be interacted with
            this.correct = new List<bool>();
        }
    }
}
