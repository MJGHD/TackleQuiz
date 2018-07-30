using System;
using Stylet;
using HandyStuff;

namespace Tackle.Pages
{
    //Initialises the ShellViewModel with the conductor interface and event handle interface
    public class ShellViewModel : Conductor<object>, IHandle<ChangePageEvent>
    {
        IEventAggregator eventAggregator;

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            //Subscribes the ShellViewModel to the event handling so that it is able to receive events raised by
            //other view models
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            ActivateItem(new LogInViewModel(this.eventAggregator));
        }

        public void Handle(ChangePageEvent eventRaised)
        {
            if(eventRaised.pageName == "LogIn")
            {
                ActivateItem(new LogInViewModel(this.eventAggregator));
            }
            else if(eventRaised.pageName == "TeacherMainMenu")
            {
                ActivateItem(new TeacherMainMenuViewModel());
            }
            /*
            else if(eventRaised.pageNumber == 2)
            {
                ActivateItem(new TeacherMenuViewModel());
            }
            */
        }
    }
}
