using Stylet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            string encryptedPassword = EncryptPassword();
        }
        public void SignUpSubmit()
        {
            Details.ButtonClickable = "False";
            string encryptedPassword = EncryptPassword();
        }

        private string EncryptPassword()
        {
            /*
            Converts the unencrypted password into a byte array, encrypts it using SHA256, and then converts it 
            back into a string
            */
            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(Details.UnencryptedPassword);
            byteArray = new System.Security.Cryptography.SHA256Managed().ComputeHash(byteArray);
            string encryptedPassword = System.Text.Encoding.ASCII.GetString(byteArray);

            return encryptedPassword;
        }
    }
}
