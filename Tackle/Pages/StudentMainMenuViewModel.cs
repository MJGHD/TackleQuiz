using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventAggr;
using Stylet;

namespace Tackle.Pages
{
    class StudentMainMenuViewModel
    {
        private IEventAggregator eventAggregator;

        public StudentMainMenuViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void JoinClass()
        {
            ChangePageEvent pageEvent = new ChangePageEvent();
            pageEvent.pageName = "JoinClass";
            this.eventAggregator.Publish(pageEvent);
        }
    }
}
