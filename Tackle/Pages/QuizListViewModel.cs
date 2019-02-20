using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventAggr;
using Networking;
using Newtonsoft.Json;
using Stylet;

namespace Tackle.Pages
{
    class QuizListViewModel
    {
        public QuizListModel Model { get; set; }

        IEventAggregator eventAggregator;

        string userType;

        public QuizListViewModel(IEventAggregator eventAggregator, string userType)
        {
            this.eventAggregator = eventAggregator;
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
            if (this.userType == "STUDENT")
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
    }
}
