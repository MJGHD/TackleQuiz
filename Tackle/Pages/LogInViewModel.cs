using Stylet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Networking;

namespace Tackle.Pages
{
    class LogInViewModel
    {
        //Property containing all of the properties in the LogInModel
        public LogInModel Details { get; set; }

        public LogInViewModel()
        {
            Details = new LogInModel();
            Details.ButtonClickable = "True";
        }

        public void LogInSubmit()
        {
            Details.ButtonClickable = "False";
            string encryptedPassword = Details.EncryptPassword(Details.UnencryptedPassword);
        }
        public void SignUpSubmit()
        {
            ServerConnection server = new ServerConnection();

            Details.ButtonClickable = "False";
            string encryptedPassword = Details.EncryptPassword(Details.UnencryptedPassword);
            string isTeacher;
            if(Details.IsTeacher)
            {
                isTeacher = "1";
            }
            else
            {
                isTeacher = "0";
            }

            string[] requestArgs = new string[] { Details.Username, encryptedPassword,isTeacher };

            bool signUpRequestSuccess = server.ServerRequest("SIGNUP", requestArgs);

            if (signUpRequestSuccess is true)
            {
                //raise event
                
            }
            else
            {
                MessageBox.Show("Sign up unsuccessful. Check internet connection or try a different username");
                Details.ButtonClickable = "True";
            }

            
        }
    }
}
