using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventAggr;
using System.Diagnostics;

namespace Tackle.Pages
{
    class JoinClassViewModel
    {
        JoinClassModel model { get; set; }
        private IEventAggregator eventAggregator;

        public JoinClassViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.model = new JoinClassModel();
        }

        public void SubmitClass()
        {
            Debug.WriteLine(this.model.ClassID);
        }
    }
}
