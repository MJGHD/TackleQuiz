using Stylet;

namespace Tackle.Pages
{
    class DraftListModel : PropertyChangedBase
    {
        public DraftListModel()
        {
            this.List = new BindableCollection<Quizzes>();
        }

        private BindableCollection<Quizzes> _list;

        public BindableCollection<Quizzes> List
        {
            get { return this._list; }
            set { SetAndNotify(ref this._list, value); }
        }
    }
}
