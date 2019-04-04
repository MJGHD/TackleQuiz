using Microsoft.Win32;
using Networking;
using Newtonsoft.Json;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz;
using EventAggr;

namespace Tackle.Pages
{
    class TeacherQuizHistoryViewModel
    {
        public TeacherQuizHistoryModel Model { get; set; }
        private IEventAggregator eventAggregator;
        string username;

        public TeacherQuizHistoryViewModel(IEventAggregator eventAggregator, string username)
        {
            this.Model = new TeacherQuizHistoryModel();
            this.eventAggregator = eventAggregator;
            this.username = username;
            //gets the info from the server
            SendRequest();
        }

        //Sends the quiz list request to the server and then fills the Model with the quiz attempts
        void SendRequest()
        {
            ServerConnection server = new ServerConnection();
            string JSON = server.ServerRequest("TEACHERQUIZHISTORY", new string[] { username });
            FillModel(JsonConvert.DeserializeObject<SetQuizResponse>(JSON));
        }

        //Takes the QuizList object and puts the values into the Model's List list
        void FillModel(SetQuizResponse setQuizResponse)
        {
            int counter = 0;

            QuizList list = setQuizResponse.attemptList;

            //prevents iteration over a null object
            if(!(setQuizResponse.quizIDs is null) || !(setQuizResponse.attemptList is null))
            {
                foreach (int quizID in list.quizIDs)
                {
                    //adds the quiz attempt
                    this.Model.QuizAttempts.Add(new Quizzes() { quizID = quizID, username = list.usernames[counter], quizContent = list.quizContents[counter], pointer = counter });
                    counter += 1;
                }

                foreach (string quizID in setQuizResponse.quizIDs)
                {
                    this.Model.List.Add(quizID);
                }
            }
        }

        public void MakeCSV(int quizID)
        {
            string CSV = BuildCSV(quizID);

            //Prompts the user to point to a save location, where the CSV will be saved
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, CSV);
            }
        }

        string BuildCSV(int quizID)
        {
            string CSV = "Username, Percentage \n ";

            //goes through each quiz attempt
            foreach(Quizzes attempt in this.Model.QuizAttempts)
            {
                //if the quiz attempt is of the same quiz ID, add the result
                if(attempt.quizID == quizID)
                {
                    string JSON = attempt.quizContent;
                    QuizInstance quizAttempt = JsonConvert.DeserializeObject<QuizInstance>(JSON);
                    //gets the percentage
                    decimal percentage = Convert.ToDecimal(quizAttempt.correctTotal) / Convert.ToDecimal(quizAttempt.questions.Length);
                    percentage *= 100;
                    //adds the result to the CSV
                    CSV += $"{quizAttempt.username},{percentage} \n ";
                }
            }
            return CSV;
        }

        public void Back()
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "TeacherMainMenu";
            this.eventAggregator.Publish(changePage);
        }
    }
}
