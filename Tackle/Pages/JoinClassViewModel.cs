using Stylet;
using EventAggr;
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
            // submits joining class request, with whether it was a success being returned as a string into joinClassAttempt
            ServerConnection server = new ServerConnection();
            string joinClassAttempt = server.ServerRequest("JOINCLASS", new string[2] { model.Username, model.ClassID.ToString() });
            //Prevents null characters from being at the end of the string, making it easier to handle
            joinClassAttempt = joinClassAttempt.Replace("\0", string.Empty);

            //if it was a success, go back to the student main menu and display a success message box
            if (joinClassAttempt == "success")
            {
                MessageBox.Show($"Successfully requested to join the class {model.ClassID}");

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
