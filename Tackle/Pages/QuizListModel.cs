using Stylet;
using System.Collections.Generic;

namespace Tackle.Pages
{
    class QuizListModel : PropertyChangedBase
    {
        public QuizListModel()
        {
            this.List = new BindableCollection<Quizzes>();
        }

        private BindableCollection<Quizzes> _list;
        private string _searchText;

        public BindableCollection<Quizzes> List
        {
            get { return this._list; }
            set { SetAndNotify(ref this._list, value); }
        }

        public string SearchText
        {
            get { return this._searchText; }
            set { SetAndNotify(ref this._searchText, value); }
        }
    }

    //class that will be used for the quiz list
    public class Quizzes
    {
        public int quizID { get; set; }
        public string username { get; set; }
        public string quizType { get; set; }
        public string quizName { get; set; }
        public string topMark { get; set; }
        //used in the TeacherQuizHistoryModel
        public string quizContent { get; set; }

        public int pointer { get; set; }
    }

    //class for the JSON to be deserialised into
    public class QuizList
    {
        public int[] quizIDs;
        public string[] usernames;
        public string[] quizType;
        public string[] quizNames;
        public string[] topMarks;
        //used in the TeacherQuizHistoryModel
        public string[] quizContents;
    }
}
