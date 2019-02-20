using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

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
    class Quizzes
    {
        public int quizID { get; set; }
        public string username { get; set; }
        public string quizType { get; set; }
        public string quizName { get; set; }
    }

    //class for the JSON to be deserialised into
    class QuizList
    {
        public int[] quizIDs;
        public string[] usernames;
        public string[] quizType;
        public string[] quizNames;
    }
}
