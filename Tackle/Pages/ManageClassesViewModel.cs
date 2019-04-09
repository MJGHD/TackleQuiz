using Networking;
using Stylet;
using System.Windows;
using Newtonsoft.Json;
using EventAggr;

namespace Tackle.Pages
{
    class ManageClassesViewModel
    {
        public ManageClassesModel Model { get; set; }

        string username;

        IEventAggregator eventAggregator;
        IWindowManager windowManager;
        
        public ManageClassesViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, string username)
        {
            this.Model = new ManageClassesModel();
            this.username = username;
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;

            RefreshList();
        }

        public void DeleteClass(int classID)
        {
            ServerConnection server = new ServerConnection();
            string success = server.ServerRequest("DELETECLASS", new string[] { classID.ToString() });
            success = success.Replace("\0", string.Empty);
            if (success == "success")
            {
                MessageBox.Show("Class deletion was successful");
            }
            else
            {
                MessageBox.Show("Class deletion wasn't successful");
            }
            RefreshList();
        }

        public void ManageClass(int classID)
        {
            //show class management dialog with the list of members
            this.windowManager.ShowDialog(new ManageClassPopUpViewModel(this.windowManager, classID));
        }

        public void CreateClass()
        {
            ServerConnection server = new ServerConnection();
            string success = server.ServerRequest("CREATECLASS", new string[] { this.username });
            success = success.Replace("\0", string.Empty);
            if(success == "success")
            {
                MessageBox.Show("Class created successfully");
                RefreshList();
            }
            else
            {
                MessageBox.Show("Class not created successfully");
            }
        }

        //accepting a class request
        public void AcceptRequest(int pointer)
        {
            ServerConnection server = new ServerConnection();
            string success = server.ServerRequest("ACCEPTREQUEST", new string[] { this.Model.Requests[pointer].username, this.Model.Requests[pointer].classID });
            success = success.Replace("\0", string.Empty);
            if (success == "success")
            {
                MessageBox.Show("Request accepted");
                RefreshList();
            }
            else
            {
                MessageBox.Show("Request not accepted");
            }
        }

        //gets the list of classes and class joining requests
        void RefreshList()
        {
            //gets the class list
            ServerConnection server = new ServerConnection();
            string JSON = server.ServerRequest("CLASSLIST", new string[] { this.username });
            //clears the current class list
            this.Model.List.Clear();
            ClassList list = new ClassList();
            list = JsonConvert.DeserializeObject<ClassList>(JSON);

            //gets the class requests
            ServerConnection server2 = new ServerConnection();
            JSON = server.ServerRequest("REQUESTLIST", new string[] { this.username });
            //clears the current class list
            this.Model.List.Clear();
            ClassRequests requests = new ClassRequests();
            requests = JsonConvert.DeserializeObject<ClassRequests>(JSON);
            FillModel(list, requests);
        }

        //fills the model with the values to be displayed by the view
        void FillModel(ClassList list, ClassRequests requests)
        {
            //clears the lists so they doesnt just append the new values on when refreshing
            this.Model.List.Clear();
            this.Model.Requests.Clear();

            int counter = 0;

            //adds the classes to the class list in the Model
            foreach (int classID in list.classIDs)
            {
                this.Model.List.Add(new Classes() { classID = classID, memberCount = list.memberCounts[counter] });
                counter += 1;
            }

            //reset counter
            counter = 0;

            //adds the class requests to the class requests in the Model
            foreach (string classID in requests.classIDs)
            {
                this.Model.Requests.Add(new Request() { classID = classID, username = requests.usernames[counter] , pointer = counter});
                counter += 1;
            }
        }

        public void Back()
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "TeacherMainMenu";
            this.eventAggregator.Publish(changePage);
        }
    }
}
