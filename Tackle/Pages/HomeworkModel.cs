using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tackle.Pages
{
    class HomeworkModel : PropertyChangedBase
    {
        public HomeworkModel()
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

}