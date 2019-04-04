using EventAggr;
using Networking;
using Newtonsoft.Json;
using Stylet;

namespace Tackle.Pages
{
    class DraftListViewModel
    {
        public DraftListModel Model { get; set; }

        IEventAggregator eventAggregator;
        IWindowManager windowManager;

        string username;

        public DraftListViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, string username)
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;
            this.username = username;

            this.Model = new DraftListModel();

            //Fetches all the quizzes to list
            RequestList();
        }

        public void Select(int quizID)
        {
            this.windowManager.ShowDialog(new QuizSelectViewModel(quizID.ToString(), this.username, true, this.eventAggregator, this.windowManager));
            RequestList();
        }

        //Sends the quiz list request to the server and then fills the Model with the quizzes
        void RequestList()
        {
            ServerConnection server = new ServerConnection();
            string JSON = server.ServerRequest("DRAFTLIST", new string[] { this.username });
            FillModel(JsonConvert.DeserializeObject<QuizList>(JSON));
        }

        //Takes the QuizList object and puts the values into the Model's List list
        void FillModel(QuizList list)
        {
            //clears the list so it doesnt just append the new values on
            this.Model.List.Clear();

            int counter = 0;

            foreach (int quizID in list.quizIDs)
            {
                this.Model.List.Add(new Quizzes() { quizID = quizID, quizName = list.quizNames[counter], quizType = list.quizType[counter], username = list.usernames[counter] });
                counter += 1;
            }
        }

        public void Back()
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "TeacherMainMenu";
            this.eventAggregator.Publish(changePage);
        }
    }
}
