using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

namespace Tackle.Pages
{
    class MarkListModel : PropertyChangedBase
    {
        //List of quiz IDs that a teacher has set
        private BindableCollection<MarkInstance> _list;

        public BindableCollection<MarkInstance> List
        {
            get { return this._list; }
            set { SetAndNotify(ref this._list, value); }
        }

        public MarkListModel()
        {
            this.List = new BindableCollection<MarkInstance>();
        }
    }
    
    //for the bindable collection to bind to
    class MarkInstance
    {
        public string username { get; set; }
        public string quizID { get; set; }
        public int pointer { get; set; }
    }

    //list of quizzes to mark
    class MarkList
    {
        public string[] usernames;
        public string[] quizIDs;
    }
}
