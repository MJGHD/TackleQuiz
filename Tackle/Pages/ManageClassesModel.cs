using Stylet;

namespace Tackle.Pages
{
    class ManageClassesModel : PropertyChangedBase
    {
        //List of classes
        private BindableCollection<Classes> _classList;

        public BindableCollection<Classes> List
        {
            get { return this._classList; }
            set { SetAndNotify(ref this._classList, value); }
        }

        //List of class requests
        private BindableCollection<Request> _classRequests;

        public BindableCollection<Request> Requests
        {
            get { return this._classRequests; }
            set { SetAndNotify(ref this._classRequests, value); }
        }

        public ManageClassesModel()
        {
            this.List = new BindableCollection<Classes>();
            this.Requests = new BindableCollection<Request>();
        }
    }

    class Classes
    {
        public int classID { get; set; }
        public int memberCount { get; set; }
    }

    class Request
    {
        public string classID { get; set; }
        public string username { get; set; }
        //this pointer is simply used to identify the request number - this is because passing multiple args through CommandParameter in the XAML is extremely difficult
        public int pointer;
    }

    //for the JSON to be deserialised into from the server response
    class ClassList
    {
        public int[] classIDs;
        public int[] memberCounts;
    }

    class ClassRequests
    {
        public string[] classIDs;
        public string[] usernames;
    }
}
