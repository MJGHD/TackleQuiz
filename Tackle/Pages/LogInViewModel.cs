using System.Windows;
using System.Windows.Controls;
using Stylet;
using System.Diagnostics;
using Networking;
using EventAggr;
using Username;

namespace Tackle.Pages
{
    //TODO: Figure out how sessions work for the username (maybe have something in the ShellViewModel that can be passed to the submit page?)
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
            //If the username and/or password haven't been entered
            if(this.Details.Username is null || this.Details.Password is null || this.Details.Password.ToString() == "" || this.Details.Username == "")
            {
                MessageBox.Show("Please enter the username and password");
            }
            else
            {
                ServerConnection server = new ServerConnection();
                //takes the parameter of the password box and makes it into an object
                var passwordBox = (PasswordBox)passwordBoxParameter;
                //can't click the buttons any more to prevent flooding the server with unnecessary requests
                Details.ButtonClickable = "False";
                //hashes the password and clears the password box object for security reasons
                string SHA256Password = Details.EncryptPassword(passwordBox.Password);
                passwordBox.Clear();

                //sends the sign up request to the server
                string[] requestArgs = new string[] { Details.Username, SHA256Password, Details.IsTeacher.ToString() };

                string signUpRequest = server.ServerRequest("SIGNUP", requestArgs);

                //if it was a success, go to the respective main menu and pass the username and type of user
                if (signUpRequest != "FAILED")
                {
                    ChangePageEvent pageEvent = new ChangePageEvent();
                    Debug.WriteLine(signUpRequest);

                    if (signUpRequest == "TEACHER")
                    {
                        pageEvent.pageName = "TeacherMainMenu";
                    }
                    else
                    {
                        pageEvent.pageName = "StudentMainMenu";
                    }

                    UsernameEvent usernameEvent = new UsernameEvent();
                    usernameEvent.username = Details.Username;
                    if (signUpRequest == "TEACHER")
                    {
                        usernameEvent.userType = "TEACHER";
                    }
                    else
                    {
                        usernameEvent.userType = "STUDENT";
                    }

                    this.eventAggregator.Publish(usernameEvent);
                    this.eventAggregator.Publish(pageEvent);
                }
                else
                {
                    MessageBox.Show("Sign up/log in unsuccessful. Please make sure that your credentials are correct, or try a different username");
                    Details.ButtonClickable = "True";
                }
            }  
        }

        public void LogInSubmit(object passwordBoxParameter)
        {
            //If the username and/or password haven't been entered
            if (this.Details.Username is null || this.Details.Password is null || this.Details.Password.ToString() == "" || this.Details.Username == "")
            {
                MessageBox.Show("Please enter the username and password");
            }
            else
            {
                ServerConnection server = new ServerConnection();
                //takes the parameter of the password box and makes it into an object
                var passwordBox = (PasswordBox)passwordBoxParameter;
                //can't click the buttons any more to prevent flooding the server with unnecessary requests
                Details.ButtonClickable = "False";
                //hashes the password and clears the password box object for security reasons
                string SHA256Password = Details.EncryptPassword(passwordBox.Password);
                passwordBox.Clear();

                //sends log in request to the server
                string[] requestArgs = new string[] { Details.Username, SHA256Password };

                string logInRequest = server.ServerRequest("LOGIN", requestArgs);

                //if it was a success, go to the respective main menu and pass the username and type of user to the ShellViewModel
                if (!logInRequest.Equals("FAILED"))
                {
                    ChangePageEvent pageEvent = new ChangePageEvent();
                    if (logInRequest == "STUDENT")
                    {
                        pageEvent.pageName = "StudentMainMenu";
                    }
                    else
                    {
                        pageEvent.pageName = "TeacherMainMenu";
                    }

                    UsernameEvent usernameEvent = new UsernameEvent();
                    usernameEvent.username = Details.Username;
                    usernameEvent.userType = logInRequest;

                    this.eventAggregator.Publish(usernameEvent);
                    this.eventAggregator.Publish(pageEvent);
                }
                else
                {
                    MessageBox.Show("Sign up/log in unsuccessful. Please make sure that your credentials are correct, or try a different username");
                    Details.ButtonClickable = "True";
                }
            }
        }
    }
}
