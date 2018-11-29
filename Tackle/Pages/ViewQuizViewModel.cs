using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz;
using Stylet;
using System.Windows;
using EventAggr;

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
        
        void SetUpQuestion()
        {
            this.Model.CurrentQuestion = this.Model.Questions[this.Model.CurrentQuestionNumber];
            this.Model.CurrentQuestionCorrect = this.Model.CorrectQuestions[this.Model.CurrentQuestionNumber];
            this.Model.CurrentUserAnswer = this.Model.Answers[this.Model.CurrentQuestionNumber];
        }
    }
}
