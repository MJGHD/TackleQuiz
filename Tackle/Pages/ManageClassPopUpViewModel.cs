using Networking;
using Newtonsoft.Json;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tackle.Pages
{
    class ManageClassPopUpViewModel : Screen
    {
        public ManageClassPopUpModel Model { get; set; }
        IWindowManager windowManager;
        int classID;

        public ManageClassPopUpViewModel(IWindowManager windowManager, int classID)
        {
            this.windowManager = windowManager;
            this.classID = classID;
            this.Model = new ManageClassPopUpModel();
            GetMemberList();
        }

        public void RemoveStudent(string username)
        {
            ServerConnection server = new ServerConnection();
            string success = server.ServerRequest("REMOVECLASSMEMBER", new string[] { this.classID.ToString(), username });
            success = success.Replace("\0", string.Empty);
            if (success == "success")
            {
                MessageBox.Show($"Removal of {username} successful");
                //refreshes list of students
                GetMemberList();
            }
            else
            {
                MessageBox.Show("Class member wasn't removed");
            }
        }

        public void GetMemberList()
        {
            //gets class members from server
            ServerConnection server = new ServerConnection();
            string JSON = server.ServerRequest("CLASSMEMBERLIST", new string[] { this.classID.ToString() });
            JSON = JSON.Replace("\0", string.Empty);

            FillModel(JSON);
        }

        //fills the model so that the members can be displayed
        void FillModel(string JSON)
        {
            //clears the current student list
            this.Model.StudentList.Clear();

            //deserialises the array of usernames that the server returns
            string[] usernames = JsonConvert.DeserializeObject<string[]>(JSON);

            foreach(string username in usernames)
            {
                this.Model.StudentList.Add(new Student(username));
            }
        }
    }
}
