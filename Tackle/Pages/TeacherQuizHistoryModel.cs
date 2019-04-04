using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

namespace Tackle.Pages
{
    class TeacherQuizHistoryModel : PropertyChangedBase
    {
        //List of quiz IDs that a teacher has set
        private BindableCollection<string> _list;

        public BindableCollection<string> List
        {
            get { return this._list; }
            set { SetAndNotify(ref this._list, value); }
        }

        //All of the quiz attempts that are included in the teacher's list of quizzes
        private List<Quizzes> _quizAttempts;

        public List<Quizzes> QuizAttempts
        {
            get { return this._quizAttempts; }
            set { SetAndNotify(ref this._quizAttempts, value); }
        }

        public TeacherQuizHistoryModel()
        {
            this.List = new BindableCollection<string>();
            this.QuizAttempts = new List<Quizzes>();
        }
    }
    //used in TeacherQuizHistory to get the list of quiz IDs that need to be displayed, and then the attempts at that quiz
    public class SetQuizResponse
    {
        public string[] quizIDs;
        public QuizList attemptList;
    }
}
