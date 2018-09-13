using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

namespace Tackle.Pages
{
    class TeacherMainMenuViewModel
    {
        private IEventAggregator eventAggregator;

        public TeacherMainMenuViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void LoadTest()
        {

        }
    }
}
