using Quiz;
using Stylet;
using System.Windows;
using EventAggr;
using System.Diagnostics;
using Networking;

namespace Tackle.Pages
{
    class ViewQuizViewModel
    {
        private IEventAggregator eventAggregator;

        public ViewQuizModel Model { get; set; }

        public ViewQuizViewModel(IEventAggregator eventAggregator, string username, int quizID)
        {
            this.eventAggregator = eventAggregator;
            this.Model = new ViewQuizModel();

            bool success;
            //gets the quiz attempt information from the server
            (Model.QuizID, Model.Username, Model.QuizType, Model.Questions, Model.Answers, Model.CorrectQuestions,success) = QuizHandling.GetQuizInformation(username,quizID);

            if(success == false)
            {
                MessageBox.Show("Something went wrong loading the quiz. Returning to the main menu");

                ChangePageEvent changePage = new ChangePageEvent();
                changePage.pageName = "TeacherMainMenu";
                this.eventAggregator.Publish(changePage);
            }
            else
            {
                this.Model.CurrentQuestionNumber = 0;
                SetUpQuestion();
            }
        }

        public void PreviousQuestion()
        {
            this.Model.CurrentQuestionNumber -= 1;

            if (this.Model.CurrentQuestionNumber.Equals(-1))
            {
                this.Model.CurrentQuestionNumber += 1;
            }

            SetUpQuestion();
        }

        public void NextQuestion()
        {
            this.Model.CorrectQuestions[Model.CurrentQuestionNumber] = this.Model.CurrentQuestionCorrect;

            this.Model.CurrentQuestionNumber += 1;

            if (this.Model.CurrentQuestionNumber == this.Model.Questions.Length)
            {
                FinishMarking();

                //if the sending to the server fails, goes back a question so that it doesn't try to access a value that isn't there
                this.Model.CurrentQuestionNumber -= 1;
            }
            else
            {
                SetUpQuestion();
            }
        }

        void SetUpQuestion()
        {
            Debug.WriteLine(Model.CurrentQuestionNumber);
            Debug.WriteLine(Model.Questions.Length-1);
            this.Model.CurrentQuestion = this.Model.Questions[this.Model.CurrentQuestionNumber];
            this.Model.CurrentQuestionCorrect = this.Model.CorrectQuestions[this.Model.CurrentQuestionNumber];
            this.Model.CurrentUserAnswer = this.Model.Answers[this.Model.CurrentQuestionNumber];

            //If it's the final question, display "Finish" instead of "next question"
            if(Model.CurrentQuestionNumber == Model.Questions.Length-1)
            {
                this.Model.NextButtonText = "Finish";
            }
            else
            {
                this.Model.NextButtonText = "Next Question";
            }
        }

        //gets the total amount of correct questions
        string GetCorrectTotal()
        {
            int total = 0;

            foreach(bool correct in this.Model.CorrectQuestions)
            {
                if (correct)
                {
                    total += 1;
                }
            }
            return total.ToString();
        }

        void FinishMarking()
        {
            string correctTotal = GetCorrectTotal();

            //updates the amount of correct questions in the QuizAttempts table
            ServerConnection server = new ServerConnection();
            string success = server.ServerRequest("FINISHMARKING", new string[] { this.Model.Username, this.Model.QuizID.ToString(), correctTotal});
            success = success.Replace("\0", string.Empty);

            if (success == "success")
            {
                MessageBox.Show("Updated the correct total");

                ChangePageEvent changePage = new ChangePageEvent();
                changePage.pageName = "TeacherMainMenu";
                this.eventAggregator.Publish(changePage);
            }
            else
            {
                MessageBox.Show("Failed to update the correct total");
            }
        }
    }
}
