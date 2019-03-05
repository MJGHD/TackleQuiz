using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventAggr;
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

        public void CreateQuiz()
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "CreateQuiz";
            this.eventAggregator.Publish(changePage);
        }

        public void ViewPublicQuizzes()
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "ViewQuizzes";
            this.eventAggregator.Publish(changePage);
        }

        public void ManageClasses()
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "ManageClasses";
            this.eventAggregator.Publish(changePage);
        }
    }
}
