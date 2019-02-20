using Results;
using Quiz;

namespace EventAggr
{
    public class ChangePageEvent
    {
        public string pageName;
        public int quizID;
        //Only used to pass the results from the Quiz page to the Quiz submit page
        public QuizResults results;
        //Used to pass quiz JSON from QuizList to QuizScreen
        public string quizJSON;
    }
}
