using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventAggr;
using MarkQuiz;
using Networking;
using Newtonsoft.Json;
using Stylet;

namespace Tackle.Pages
{
    class MarkListViewModel
    {
        IEventAggregator eventAggregator;
        string username;
        public MarkListModel Model { get; set; }

        public MarkListViewModel(IEventAggregator eventAggregator, string username)
        {
            this.eventAggregator = eventAggregator;
            this.username = username;
            this.Model = new MarkListModel();
            SendQuizRequest();
        }

        //Sends the quiz list request to the server and then fills the Model with the quizzes
        void SendQuizRequest()
        {
            ServerConnection server = new ServerConnection();
            string JSON = server.ServerRequest("QUIZMARKINGLIST", new string[] { this.username });
            FillModel(JsonConvert.DeserializeObject<MarkList>(JSON));
        }

        //Takes the QuizList object and puts the values into the Model's List list
        void FillModel(MarkList list)
        {
            int counter = 0;

            foreach (string quizID in list.quizIDs)
            {
                this.Model.List.Add(new MarkInstance() { username = list.usernames[counter], quizID = list.quizIDs[counter], pointer = counter });
                counter += 1;
            }
        }

        public void Back()
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "TeacherMainMenu";
            this.eventAggregator.Publish(changePage);
        }

        public void MarkQuiz(int pointer)
        {
            //publishes the markQuiz event containing the quiz ID and the username of the quiz attempt
            MarkQuizEvent markQuiz = new MarkQuizEvent();
            markQuiz.quizID = Int32.Parse(this.Model.List[pointer].quizID);
            markQuiz.username = this.Model.List[pointer].username;
            this.eventAggregator.Publish(markQuiz);

            //changes page to the quiz marking
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "MarkQuizView";
            this.eventAggregator.Publish(changePage);
        }
    }
}
