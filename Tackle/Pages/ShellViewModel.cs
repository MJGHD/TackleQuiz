using Stylet;
using EventAggr;
using Username;
using MarkQuiz;

namespace Tackle.Pages
{
    //Initialises the ShellViewModel with the conductor interface and event handle interfaces
    public class ShellViewModel : Conductor<object>, IHandle<ChangePageEvent>, IHandle<UsernameEvent>, IHandle<MarkQuizEvent>
    {
        IEventAggregator eventAggregator;
        IWindowManager windowManager;

        string username;
        string userType;

        MarkQuizEvent markQuiz;

        public ShellViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            //Subscribes the ShellViewModel to the event handling so that it is able to receive events raised by
            //other view models
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            //initialises the window manager to be used when needed
            this.windowManager = windowManager;

            ActivateItem(new LogInViewModel(this.eventAggregator));
        }

        //Handles when a page publishes a ChangePageEvent event
        public void Handle(ChangePageEvent eventRaised)
        {
            //activates the appropriate view model
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
                case "TakeQuiz":
                    ActivateItem(new QuizScreenViewModel(this.eventAggregator, eventRaised.quizID, this.userType, this.username));
                    break;
                case "QuizSubmit":
                    ActivateItem(new QuizSubmitViewModel(this.eventAggregator,eventRaised.results));
                    break;
                case "JoinClass":
                    ActivateItem(new JoinClassViewModel(this.eventAggregator, this.username));
                    break;
                case "ManageClasses":
                    ActivateItem(new ManageClassesViewModel(this.eventAggregator, this.windowManager, this.username));
                    break;
                case "CreateQuiz":
                    ActivateItem(new CreateQuizViewModel(this.eventAggregator, this.username, this.windowManager, -1));
                    break;
                case "ViewQuizzes":
                    ActivateItem(new QuizListViewModel(this.eventAggregator, this.windowManager, this.userType, this.username));
                    break;
                case "DraftList":
                    ActivateItem(new DraftListViewModel(this.eventAggregator, this.windowManager, this.username));
                    break;
                case "HomeworkList":
                    ActivateItem(new HomeworkViewModel(this.eventAggregator, this.windowManager, this.username));
                    break;
                case "ViewSetQuizzes":
                    ActivateItem(new TeacherQuizHistoryViewModel(this.eventAggregator, this.username));
                    break;
                case "EditQuiz":
                    ActivateItem(new CreateQuizViewModel(this.eventAggregator, this.username, this.windowManager, eventRaised.quizID));
                    break;
                case "MarkQuizList":
                    ActivateItem(new MarkListViewModel(this.eventAggregator, this.username));
                    break;
                case "MarkQuizView":
                    ActivateItem(new ViewQuizViewModel(this.eventAggregator,this.markQuiz.username, this.markQuiz.quizID));
                    break;
            }
        }
        
        //Handles when the LogInViewModel publishes the UsernameEvent event
        public void Handle(UsernameEvent usernameEvent)
        {
            this.username = usernameEvent.username;
            this.userType = usernameEvent.userType;
        }

        //Handles when the quizID is published from the MarkListViewModel
        public void Handle(MarkQuizEvent markQuiz)
        {
            this.markQuiz = markQuiz;
        }
    }
}
