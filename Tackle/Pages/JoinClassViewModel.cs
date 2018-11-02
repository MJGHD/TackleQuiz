using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventAggr;
using System.Diagnostics;
using Networking;
using System.Windows;

namespace Tackle.Pages
{
    class JoinClassViewModel
    {
        public JoinClassModel model { get; set; }
        private IEventAggregator eventAggregator;

        public JoinClassViewModel(IEventAggregator eventAggregator, string username)
        {
            this.eventAggregator = eventAggregator;
            this.model = new JoinClassModel();
            model.Username = username;
        }

        public void SubmitClass()
        {
            ServerConnection server = new ServerConnection();
            string joinClassAttempt = server.ServerRequest("JOINCLASS", new string[2] { model.Username, model.ClassID.ToString() });
            //Prevents null characters from being at the end of the string, making it easier to handle
            joinClassAttempt = joinClassAttempt.Replace("\0", string.Empty);

            if (joinClassAttempt == "success")
            {
                MessageBox.Show($"Successfully joined the class {model.ClassID}");

                ChangePageEvent changePage = new ChangePageEvent();
                changePage.pageName = "StudentMainMenu";
                this.eventAggregator.Publish(changePage);
            }
            else
            {
                MessageBox.Show($"Joining of {model.ClassID} unsuccessful. You may have entered the wrong ID or may already be in this class");
            }
        }
    }
}
