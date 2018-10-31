using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Username
{
    //Used to pass the username form the log in page to the ShellViewModel so that it can then be passed to any viewmodel
    //that requires it
    public class UsernameEvent
    {
        public string username;
    }
}
