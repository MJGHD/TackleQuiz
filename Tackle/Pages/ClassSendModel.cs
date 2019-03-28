using Stylet;

namespace Tackle.Pages
{
    class ClassSendModel : PropertyChangedBase
    {
        private string _classID;

        public string ClassID
        {
            get { return this._classID; }
            set { SetAndNotify(ref this._classID, value); }
        }
    }
}
