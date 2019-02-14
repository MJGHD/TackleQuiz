using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz;
using Stylet;
using System.Windows;
using EventAggr;
using System.Diagnostics;

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

        void FinishMarking()
        {
            Debug.WriteLine(this.Model.CorrectQuestions[this.Model.CorrectQuestions.Length-1]);
        }
    }
}
