using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tackle.Pages
{
    class LogInModel : PropertyChangedBase
    {
        private string _username;
        private string _unencryptedPassword;
        private string _buttonClickable;

        public string Username
        {
            get { return this._username; }
            set { SetAndNotify(ref this._username, value); }
        }

        public string UnencryptedPassword
        {
            get { return this._unencryptedPassword; }
            set { SetAndNotify(ref this._unencryptedPassword, value); }
        }

        public string ButtonClickable
        {
            get { return this._buttonClickable; }
            set { SetAndNotify(ref this._buttonClickable, value); }
        }
    }
}
