using Networking;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Tackle.Pages
{
    class ManageClassesViewModel
    {
        public ManageClassesModel Model { get; set; }

        string username;

        IEventAggregator eventAggregator;

        public ManageClassesViewModel(IEventAggregator eventAggregator, string username)
        {
            this.Model = new ManageClassesModel();
            this.username = username;
            this.eventAggregator = eventAggregator;

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

        void RefreshList()
        {
            ServerConnection server = new ServerConnection();
            string JSON = server.ServerRequest("CLASSLIST", new string[] { this.username });
            this.Model.List.Clear();
            ClassList list = new ClassList();
            list = JsonConvert.DeserializeObject<ClassList>(JSON);
            FillModel(list);
        }

        void FillModel(ClassList list)
        {
            //clears the list so it doesnt just append the new values on when refreshing
            this.Model.List.Clear();

            int counter = 0;

            foreach (int classID in list.classIDs)
            {
                this.Model.List.Add(new Classes() { classID = classID, memberCount = list.memberCounts[counter] });
                counter += 1;
            }
        }
        //TODO: Class requests
    }
}
