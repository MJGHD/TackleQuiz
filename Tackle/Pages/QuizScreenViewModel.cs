using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Tackle.HandyStuff;

namespace Tackle.Pages
{
    class QuizScreenViewModel
    {
        int quizID;
        public QuizScreenModel Model { get; set; }
        DispatcherTimer timer;

        public QuizScreenViewModel(int quizID)
        {
            this.quizID = quizID;
            this.Model = new QuizScreenModel();
            SetTimerSettings();
            (Model.Questions,Model.QuestionTypes,Model.Answers,Model.QuizType,Model.TimeLeft) = QuizHandling.OpenQuiz();
            SetFirstQuestion();
            timer.Start();
        }

        public void TimerTick(object sender, EventArgs e)
        {
            Model.TimeLeft -= 1;
            Model.TimeLeftDisplay = $"Time left: {Model.TimeLeft} seconds";
        }

        public void SetTimerSettings()
        {
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        public void SetQuestionType()
        {
            if (Model.QuestionTypes[Model.CurrentQuestionNumber] == "StringInput" || Model.QuestionTypes[Model.CurrentQuestionNumber] == "IntegerInput")
            {
                Model.CurrentQuestionType = 0;
            }
            else if (Model.QuestionTypes[Model.CurrentQuestionNumber] == "MultipleChoice")
            {
                Model.CurrentQuestionType = 1;
            }
        }

        public void NextQuestion()
        {
            Model.CurrentQuestionNumber += 1;
            Model.QuestionNumberDisplay = $"Question {Model.CurrentQuestionNumber+1}/{Model.Questions.Length}";
            SetQuestionType();
            DisplayQuestion();
        }

        public void DisplayQuestion()
        {
            Model.CurrentQuestion = Model.Questions[Model.CurrentQuestionNumber];
            if(Model.QuizType == "Instant")
            {
                Model.CurrentAnswer = Model.Answers[Model.CurrentQuestionNumber];
            }
        }

        public void SetFirstQuestion()
        {
            Model.CurrentQuestionNumber = 0;
            Model.QuestionNumberDisplay = $"Question {Model.CurrentQuestionNumber + 1}/{Model.Questions.Length}";
            SetQuestionType();
            DisplayQuestion();
        }
    }
}
