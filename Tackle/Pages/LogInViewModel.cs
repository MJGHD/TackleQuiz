﻿using System.Windows;
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
                Debug.WriteLine(signUpRequest);
                
                if(signUpRequest == "TEACHER")
                {
                    pageEvent.pageName = "TeacherMainMenu";
                }
                else
                {
                    pageEvent.pageName = "StudentMainMenu";
                }

                UsernameEvent usernameEvent = new UsernameEvent();
                usernameEvent.username = Details.Username;
                if(signUpRequest == "TEACHER")
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
