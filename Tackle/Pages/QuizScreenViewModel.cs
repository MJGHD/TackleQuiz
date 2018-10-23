using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Stylet;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Quiz;
using EventAggr;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;

namespace Tackle.Pages
{
    class QuizScreenViewModel
    {
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
            (Model.Questions,Model.QuestionTypes,Model.Answers,Model.TimeLeft) = QuizHandling.OpenQuiz();
            //TEMPORARY NEXT LINE
            Model.QuizType = "Instant";
            Model.UserInputs = new string[Model.Questions.Length];
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

        public void SaveCurrentAnswer()
        {
            Model.UserInputs[Model.CurrentQuestionNumber] = Model.UserInput;
        }

        public void SetCurrentAnswer()
        {
            Model.UserInput = Model.UserInputs[Model.CurrentQuestionNumber];
        }

        public void NextQuestion()
        {
            SaveCurrentAnswer();

            //If the quiz has finished
            if(Model.NextButtonText == "Finish Quiz")
            {
                FinishQuiz();
            }
            else
            {
                Model.CurrentQuestionNumber += 1;
                SetCurrentAnswer();
                if (Model.CurrentQuestionNumber + 1 == Model.Questions.Length)
                {
                    Model.NextButtonText = "Finish Quiz";
                }
                else
                {
                    Model.NextButtonText = "Next Question";
                }

                Model.QuestionNumberDisplay = $"Question {Model.CurrentQuestionNumber + 1}/{Model.Questions.Length}";
                SetQuestionType();
                DisplayQuestion();
            }
        }

        public void PreviousQuestion()
        {
            SaveCurrentAnswer();
            Model.NextButtonText = "Next Question";
            Model.CurrentQuestionNumber -= 1;
            SetCurrentAnswer();
            Model.QuestionNumberDisplay = $"Question {Model.CurrentQuestionNumber + 1}/{Model.Questions.Length}";
            SetQuestionType();
            DisplayQuestion();
        }

        void DisplayQuestion()
        {
            Model.CurrentQuestion = Model.Questions[Model.CurrentQuestionNumber];
            if(Model.CurrentQuestionType == 2)
            {
                GetMultipleChoices();
            }

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

        //Checks whether the user input on an integer input question is an integer - if false, the TextBox will reject the input
        public void NumericalInputFilter(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsNumber(e.Text);
        }

        //Checks whether the user's input is a number from 0 to 9
        bool IsNumber(string userInput)
        {
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(userInput);
        }

        void FinishQuiz()
        {
            if(Model.QuizType == "Instant")
            {
                int pointer = 0;
                int correctAnswers = 0;

                foreach(string answer in Model.Answers)
                {
                    //String input or multiple choice
                    if(Model.CurrentQuestionType == 0 || Model.CurrentQuestionType == 2)
                    {
                        if(Model.UserInputs[pointer] == Model.Answers[pointer])
                        {
                            correctAnswers += 1;
                        }
                    }
                    //Integer input
                    else if (Model.CurrentQuestionType == 1)
                    {
                        if (Int32.Parse(Model.UserInputs[pointer]) == Int32.Parse(Model.Answers[pointer]))
                        {
                            correctAnswers += 1;
                        }
                    }

                    pointer += 1;
                }
                MessageBox.Show(correctAnswers.ToString());

                ChangePageEvent changePage = new ChangePageEvent();
                changePage.pageName = "StudentMainMenu";
                this.eventAggregator.Publish(changePage);
            }
        }

        //Gets the multiple choice options from the questions
        void GetMultipleChoices()
        {
            List<string> choices = new List<string> { };

            //Gets the indexes of the beginning and end of the choices
            int choiceStart = Model.CurrentQuestion.IndexOf('{');
            int choiceEnd = Model.CurrentQuestion.IndexOf('}');

            //Removes the start and end of the end of the choices and then adds the choices to the options array
            string choiceString = Model.CurrentQuestion.Remove(0, choiceStart+1);
            choiceString = choiceString.TrimEnd('}');

            Model.MultipleChoiceOptions = choiceString.Split(',');
        }
    }
}
