using System.Windows;
using HandyStuff;
using System.Windows.Controls;
using Stylet;

namespace Tackle.Pages
{
    class LogInViewModel
    {
        //Property containing all of the properties in the LogInModel
        public LogInModel Details { get; set; }

        private IEventAggregator eventAggregator;

        public LogInViewModel(IEventAggregator eventAggregator)
        {
            Details = new LogInModel();
            Details.ButtonClickable = "True";
            this.eventAggregator = eventAggregator;
        }

        public void LogInSubmit()
        {
            Details.ButtonClickable = "False";
        }

        public void SignUpSubmit(object passwordBoxParameter)
        {
            ServerConnection server = new ServerConnection();
            var passwordBox = (PasswordBox)passwordBoxParameter;
            Details.ButtonClickable = "False";
            string SHA256Password = Details.EncryptPassword(passwordBox.Password);
            passwordBox.Clear();

            string isTeacher;
            if(Details.IsTeacher)
            {
                isTeacher = "1";
            }
            else
            {
                isTeacher = "0";
            }

            string[] requestArgs = new string[] { Details.Username, SHA256Password,isTeacher };

            bool signUpRequestSuccess = server.ServerRequest("SIGNUP", requestArgs);

            if (signUpRequestSuccess is true)
            {
                ChangePageEvent pageEvent = new ChangePageEvent();
                if(isTeacher == "1")
                {
                    pageEvent.pageName = "TeacherMainMenu";
                }
                else
                {
                    pageEvent.pageName = "StudentMainMenu";
                }
                this.eventAggregator.Publish(pageEvent);
            }
            else
            {
                MessageBox.Show("Sign up unsuccessful. Check internet connection or try a different username");
                Details.ButtonClickable = "True";
            }

            
        }
    }
}
