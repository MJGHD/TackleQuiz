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

        public void ViewQuizzes()
        {
            ChangePageEvent pageEvent = new ChangePageEvent();
            pageEvent.pageName = "ViewQuizzes";
            this.eventAggregator.Publish(pageEvent);
        }

        public void ViewHomework()
        {
            ChangePageEvent pageEvent = new ChangePageEvent();
            pageEvent.pageName = "HomeworkList";
            this.eventAggregator.Publish(pageEvent);
        }
    }
}
