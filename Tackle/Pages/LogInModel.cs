using Stylet;
using System.Security;

namespace Tackle.Pages
{
    class LogInModel : PropertyChangedBase
    {
        //Property declaration
        private string _username;
        private SecureString _password;
        private string _buttonClickable;
        private bool _isTeacher;

        public string Username
        {
            get { return this._username; }
            set { SetAndNotify(ref this._username, value); }
        }

        public SecureString Password
        {
            get { return this._password; }
            set { SetAndNotify(ref this._password, value); }
        }

        public string ButtonClickable
        {
            get { return this._buttonClickable; }
            set { SetAndNotify(ref this._buttonClickable, value); }
        }

        public bool IsTeacher
        {
            get { return _isTeacher; }
            set { SetAndNotify(ref this._isTeacher, value); }
        }


        //Business logic
        public string EncryptPassword(string unencryptedPassword)
        {
            /*
            Converts the unencrypted password into a byte array, hashes it using SHA256, and then converts it 
            back into a string
            */

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(unencryptedPassword);
            byteArray = new System.Security.Cryptography.SHA256Managed().ComputeHash(byteArray);
            string encryptedPassword = System.Text.Encoding.ASCII.GetString(byteArray);

            return encryptedPassword;
        }
    }
}
