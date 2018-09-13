using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void LoadTest()
        {

        }
    }
}
