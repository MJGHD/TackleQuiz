using EventAggr;
using Networking;
using Newtonsoft.Json;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tackle.Pages
{
    class HomeworkViewModel
    {
        public QuizListModel Model { get; set; }

        IEventAggregator eventAggregator;
        IWindowManager windowManager;

        string username;

        public HomeworkViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, string username)
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;
            this.username = username;

            this.Model = new QuizListModel();

            //Fetches all the quizzes to list
            SendQuizRequest();
        }

        //Sends the quiz list request to the server and then fills the Model with the quizzes
        void SendQuizRequest()
        {
            ServerConnection server = new ServerConnection();
            string JSON = server.ServerRequest("HOMEWORKLIST", new string[] { this.username });
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
                this.Model.List.Add(new Quizzes() { quizID = quizID, quizName = list.quizNames[counter], quizType = list.quizType[counter], username = list.usernames[counter], topMark = list.topMarks[counter]});
                counter += 1;
            }
        }

        //select button logic
        public void Select(int quizID)
        {
            TakeQuiz(quizID);
        }

        // makes it so the student plays the quiz, with the ID of the quiz being passed to QuizScreenViewModel
        void TakeQuiz(int quizID)
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "TakeQuiz";
            changePage.quizID = quizID;
            this.eventAggregator.Publish(changePage);
        }

        public void Back()
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "StudentMainMenu";
            this.eventAggregator.Publish(changePage);
        }
    }
}
