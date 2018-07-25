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
            string encryptedPassword = Details.EncryptPassword(Details.UnencryptedPassword);
        }
        public void SignUpSubmit()
        {
            Details.ButtonClickable = "False";
            string encryptedPassword = Details.EncryptPassword(Details.UnencryptedPassword);
        }

        
    }
}
