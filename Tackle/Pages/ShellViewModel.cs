using System;
using Stylet;
using System.Diagnostics;
using EventAggr;

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

            //ActivateItem(new QuizScreenViewModel(0));
        }

        public void Handle(ChangePageEvent eventRaised)
        {
            switch (eventRaised.pageName)
            {
                case "LogIn":
                    ActivateItem(new LogInViewModel(this.eventAggregator));
                    break;
                case "TeacherMainMenu":
                    ActivateItem(new TeacherMainMenuViewModel(this.eventAggregator));
                    break;
                case "StudentMainMenu":
                    ActivateItem(new StudentMainMenuViewModel(this.eventAggregator));
                    break;
                case "TestQuiz":
                    ActivateItem(new QuizScreenViewModel(0, this.eventAggregator));
                    break;
                case "QuizSubmit":
                    ActivateItem(new QuizSubmitViewModel(this.eventAggregator,eventRaised.results));
                    break;
            }
        }
    }
}
