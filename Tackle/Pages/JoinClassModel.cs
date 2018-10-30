using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

namespace Tackle.Pages
{
    class JoinClassModel
    {
        private int _classID;

        public int ClassID
        {
            get { return this._classID; }
            set { SetAndNotify(ref this._classID, value); }
        }
    }
}
