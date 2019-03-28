using Stylet;

namespace Tackle.Pages
{
    class JoinClassModel: PropertyChangedBase
    {
        private int _classID;
        private string _username;

        public int ClassID
        {
            get { return this._classID; }
            set { SetAndNotify(ref this._classID, value); }
        }

        public string Username
        {
            get { return this._username; }
            set { SetAndNotify(ref this._username, value); }
        }
    }
}
