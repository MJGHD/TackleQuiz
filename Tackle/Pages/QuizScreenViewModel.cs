using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using HandyStuff;
using Stylet;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Tackle.Pages
{
    class QuizScreenViewModel
    {
        //TODO: Make it so that when a character is added, it calls a function that checks whether it's a number (for the int input)
        int quizID;
        public QuizScreenModel Model { get; set; }
        DispatcherTimer timer;
        private IEventAggregator eventAggregator;

        public QuizScreenViewModel(int quizID, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.quizID = quizID;
            this.Model = new QuizScreenModel();
            SetTimerSettings();
            (Model.Questions,Model.QuestionTypes,Model.Answers,Model.QuizType,Model.TimeLeft) = QuizHandling.OpenQuiz();
            this.Model.NextButtonText = "Next Question";
            SetFirstQuestion();
            timer.Start();
        }

        void TimerTick(object sender, EventArgs e)
        {
            Model.TimeLeft -= 1;
            if(Model.TimeLeft == 0)
            {
                FinishQuiz();
            }
            else
            {
                Model.TimeLeftDisplay = $"Time left: {Model.TimeLeft} seconds";
            }
        }

        void SetTimerSettings()
        {
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        void SetQuestionType()
        {
            if (Model.QuestionTypes[Model.CurrentQuestionNumber] == "StringInput")
            {
                Model.CurrentQuestionType = 0;
            }
            if(Model.QuestionTypes[Model.CurrentQuestionNumber] == "IntegerInput")
            {
                Model.CurrentQuestionType = 1;
            }
            else if (Model.QuestionTypes[Model.CurrentQuestionNumber] == "MultipleChoice")
            {
                Model.CurrentQuestionType = 2;
            }
        }

        public void NextQuestion()
        {
            Model.CurrentQuestionNumber += 1;
            if(Model.CurrentQuestionNumber+1 == Model.Questions.Length)
            {
                Model.NextButtonText = "Finish Quiz";
            }
            else
            {
                Model.NextButtonText = "Next Question";
            }

            if(Model.CurrentQuestionNumber == Model.Questions.Length)
            {
                FinishQuiz();
            }
            else
            {
                Model.QuestionNumberDisplay = $"Question {Model.CurrentQuestionNumber + 1}/{Model.Questions.Length}";
                SetQuestionType();
                DisplayQuestion();
            }

        }

        public void PreviousQuestion()
        {
            Model.NextButtonText = "Next Question";
            Model.CurrentQuestionNumber -= 1;
            Model.QuestionNumberDisplay = $"Question {Model.CurrentQuestionNumber + 1}/{Model.Questions.Length}";
            SetQuestionType();
            DisplayQuestion();
        }

        void DisplayQuestion()
        {
            Model.CurrentQuestion = Model.Questions[Model.CurrentQuestionNumber];
            if(Model.QuizType == "Instant")
            {
                Model.CurrentAnswer = Model.Answers[Model.CurrentQuestionNumber];
            }
        }

        void SetFirstQuestion()
        {
            Model.CurrentQuestionNumber = 0;
            Model.QuestionNumberDisplay = $"Question {Model.CurrentQuestionNumber + 1}/{Model.Questions.Length}";
            SetQuestionType();
            DisplayQuestion();
        }

        public void NumericalInputFilter(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsNumber(e.Text);
        }

        bool IsNumber(string userInput)
        {
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(userInput);
        }

        void FinishQuiz()
        {
            ChangePageEvent pageEvent = new ChangePageEvent();
            pageEvent.pageName = "StudentMainMenu";
            this.eventAggregator.Publish(pageEvent);
        }
    }
}
