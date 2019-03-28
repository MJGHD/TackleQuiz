using Stylet;

namespace Tackle.Pages
{
    class ManageClassesModel : PropertyChangedBase
    {
        private BindableCollection<Classes> _classList;

        public BindableCollection<Classes> List
        {
            get { return this._classList; }
            set { SetAndNotify(ref this._classList, value); }
        }

        public ManageClassesModel()
        {
            this.List = new BindableCollection<Classes>();
        }
    }

    class Classes
    {
        public int classID { get; set; }
        public int memberCount { get; set; }
    }

    //for the JSON to be deserialised into from the server response
    class ClassList
    {
        public int[] classIDs;
        public int[] memberCounts;
    }
}
