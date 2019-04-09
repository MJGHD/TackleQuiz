using EventAggr;
using Networking;
using Newtonsoft.Json;
using Stylet;
using System.Diagnostics;

namespace Tackle.Pages
{
    class QuizListViewModel
    {
        public QuizListModel Model { get; set; }

        IEventAggregator eventAggregator;
        IWindowManager windowManager;

        string userType;
        string username;

        public QuizListViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, string userType, string username)
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;
            this.username = username;
            this.userType = userType;

            this.Model = new QuizListModel();

            //Fetches all the quizzes to list
            SendQuizRequest("");
        }

        //Sends the quiz list request to the server and then fills the Model with the quizzes
        void SendQuizRequest(string quizName)
        {
            ServerConnection server = new ServerConnection();
            string JSON = server.ServerRequest("QUIZLIST", new string[] { quizName });
            FillModel(JsonConvert.DeserializeObject<QuizList>(JSON));
        }

        //Takes the QuizList object and puts the values into the Model's List list
        void FillModel(QuizList list)
        {
            //clears the list so it doesnt just append the new values on
            this.Model.List.Clear();

            int counter = 0;

            foreach(int quizID in list.quizIDs)
            {
                this.Model.List.Add(new Quizzes() { quizID = quizID, quizName = list.quizNames[counter], quizType = list.quizType[counter], username = list.usernames[counter] });
                counter += 1;
            }
        }

        public void Select(int quizID)
        {
            //if the user is a teacher, show the quiz select pop up
            if (this.userType == "TEACHER")
            {
                this.windowManager.ShowDialog(new QuizSelectViewModel(quizID.ToString(), this.username, false, this.eventAggregator, this.windowManager));
                //if the search text isn't blank, then refresh the list of quizzes with the search query. if it is, then a blank search query is requested
                if (!(this.Model.SearchText is null))
                {
                    SendQuizRequest(this.Model.SearchText);
                }
                else
                {
                    SendQuizRequest("");
                }
            }
            //if the user is a student, take the quiz normally
            else
            {
                TakeQuiz(quizID);
            }
        }

        public void Search()
        {
            SendQuizRequest(this.Model.SearchText);
        }

        void TakeQuiz(int quizID)
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "TakeQuiz";
            changePage.quizID = quizID;
            this.eventAggregator.Publish(changePage);
        }

        public void Back()
        {
            //back button takes the user back to their respective main menu
            ChangePageEvent changePage = new ChangePageEvent();
            if(this.userType == "STUDENT")
            {
                changePage.pageName = "StudentMainMenu";
            }
            else
            {
                changePage.pageName = "TeacherMainMenu";
            }
            this.eventAggregator.Publish(changePage);
        }
    }
}
