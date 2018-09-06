using System.Windows;
using HandyStuff;
using System.Windows.Controls;
using Stylet;
using System.Diagnostics;

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

        public void SignUpSubmit(object passwordBoxParameter)
        {
            ServerConnection server = new ServerConnection();
            var passwordBox = (PasswordBox)passwordBoxParameter;
            Details.ButtonClickable = "False";
            string SHA256Password = Details.EncryptPassword(passwordBox.Password);
            passwordBox.Clear();

            string[] requestArgs = new string[] { Details.Username, SHA256Password,Details.IsTeacher.ToString() };

            string signUpRequest = server.ServerRequest("SIGNUP", requestArgs);

            if (signUpRequest != "FAILED")
            {
                ChangePageEvent pageEvent = new ChangePageEvent();
                if(signUpRequest == "True")
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

        public void LogInSubmit(object passwordBoxParameter)
        {
            ServerConnection server = new ServerConnection();
            var passwordBox = (PasswordBox)passwordBoxParameter;
            Details.ButtonClickable = "False";
            string SHA256Password = Details.EncryptPassword(passwordBox.Password);
            passwordBox.Clear();

            string[] requestArgs = new string[] { Details.Username, SHA256Password };

            string logInRequest = server.ServerRequest("LOGIN", requestArgs);

            if (!logInRequest.Equals("FAILED"))
            {
                ChangePageEvent pageEvent = new ChangePageEvent();
                if (logInRequest.Equals("0"))
                {
                    pageEvent.pageName = "StudentMainMenu";
                }
                else
                {
                    pageEvent.pageName = "TeacherMainMenu";
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
