using System;
using Stylet;
using System.Diagnostics;
using EventAggr;
using Username;
using MarkQuiz;

namespace Tackle.Pages
{
    //Initialises the ShellViewModel with the conductor interface and event handle interfaces
    public class ShellViewModel : Conductor<object>, IHandle<ChangePageEvent>, IHandle<UsernameEvent>
    {
        IEventAggregator eventAggregator;

        string username;
        MarkQuizEvent markQuiz;

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            //Subscribes the ShellViewModel to the event handling so that it is able to receive events raised by
            //other view models
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            ActivateItem(new ViewQuizViewModel(this.eventAggregator, "teststudent", 0));
            //ActivateItem(new LogInViewModel(this.eventAggregator));
        }

        //Handles when a page publishes a ChangePageEvent event
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
                case "JoinClass":
                    ActivateItem(new JoinClassViewModel(this.eventAggregator, username));
                    break;
                //case "MarkQuizList":
                //    ActivateItem(new MarkQuizListViewModel(this.eventAggregator));
                //    break;
                case "MarkQuizView":
                    ActivateItem(new ViewQuizViewModel(this.eventAggregator,this.markQuiz.username, this.markQuiz.quizID));
                    break;
            }
        }
        
        //Handles when the LogInViewModel publishes the UsernameEvent event
        public void Handle(UsernameEvent usernameEvent)
        {
            this.username = usernameEvent.username;
        }

        //Handles when the quizID is published from the MarkListViewModel
        public void Handle(MarkQuizEvent markQuiz)
        {
            this.markQuiz = markQuiz;
        }
    }
}
