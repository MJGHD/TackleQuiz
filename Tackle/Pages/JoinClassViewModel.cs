using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventAggr;
using System.Diagnostics;
using Networking;

namespace Tackle.Pages
{
    class JoinClassViewModel
    {
        public JoinClassModel model { get; set; }
        private IEventAggregator eventAggregator;

        public JoinClassViewModel(IEventAggregator eventAggregator, string username)
        {
            this.eventAggregator = eventAggregator;
            this.model = new JoinClassModel();
            model.Username = username;
        }

        public void SubmitClass()
        {
            ServerConnection server = new ServerConnection();
            server.ServerRequest("JOINCLASS", new string[2] { model.Username, model.ClassID.ToString() });
        }
    }
}
