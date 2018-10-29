using Results;

namespace EventAggr
{
    public class ChangePageEvent
    {
        public string pageName;
        public int quizID;
        //Only used to pass the results from the Quiz page to the Quiz submit page
        public QuizResults results;
    }
}
