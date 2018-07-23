using System;
using Stylet;

namespace Tackle.Pages
{
    //Initialises the ShellViewModel with the conductor interface and event handle interface
    public class ShellViewModel : Conductor<object>, IHandle<ChangePageEvent>
    {
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            //Subscribes the ShellViewModel to the event handling so that it is able to receive events raised by
            //other view models
            eventAggregator.Subscribe(this);
            ActivateItem(new LogInViewModel());
        }

        public void Handle(ChangePageEvent eventRaised)
        {
            if(eventRaised.pageNumber == 0)
            {
                ActivateItem(new LogInViewModel());
            }
            /*
            else if(eventRaised.pageNumber == 1)
            {
                ActivateItem(new StudentMenuViewModel());
            }
            else if(eventRaised.pageNumber == 2)
            {
                ActivateItem(new TeacherMenuViewModel());
            }
            */
        }
    }

    public class ChangePageEvent
    {
        public int pageNumber;
    }
}
