using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Stylet;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Quiz;
using EventAggr;
using Results;
using Networking;
using JSON;
using System.Windows;
using System.Diagnostics;
using System.Timers;

namespace Tackle.Pages
{
    class QuizScreenViewModel
    {
        int quizID;
        string userType;
        string username;
        public QuizScreenModel Model { get; set; }
        Timer timer;
        private IEventAggregator eventAggregator;

        public QuizScreenViewModel(IEventAggregator eventAggregator, int quizID, string userType, string username)
        {
            this.eventAggregator = eventAggregator;
            this.quizID = quizID;
            this.userType = userType;
            this.username = username;
            this.Model = new QuizScreenModel();

            //gets the quiz JSON and then fills the model with the quiz's content
            string JSON = QuizHandling.GetQuizJSON(this.quizID);
            (Model.Questions, Model.QuestionTypes, Model.Answers, Model.TimeLeft, Model.QuizType) = QuizHandling.OpenQuiz(JSON);

            //creates a string array that the user's inputs will go into
            Model.UserInputs = new string[Model.Questions.Length];
            this.Model.NextButtonText = "Next Question";
            SetFirstQuestion();

            SetTimerSettings();
        }

        void TimerTick(object sender, EventArgs e)
        {
            Model.TimeLeft -= 1;
            //If time has finished, finish the quiz
            if(Model.TimeLeft < 1)
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
            //Makes it so that the timer ticks every second, and then every second it called the TimerTick function
            timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(TimerTick);
            timer.Start();
        }

        void SetQuestionType()
        {
            Model.CurrentQuestionType = Model.QuestionTypes[Model.CurrentQuestionNumber];
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
                //increase the question number by one and then set the question's information
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
            //saves the current answer and then goes onto the next question
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

            //Stops values from being null if they are a new question
            if (Model.CurrentQuestionType == 0)
            {
                if(Model.UserInput is null)
                {
                    Model.UserInput = "";
                }
            }
            else if(Model.CurrentQuestionType == 1)
            {
                if (Model.UserInput is null)
                {
                    Model.UserInput = "0";
                }
            }
            //Gets the multiple choices
            else if(Model.CurrentQuestionType == 2)
            {
                GetMultipleChoices();
            }

            if(Model.QuizType == "Instant")
            {
                Model.CurrentAnswer = Model.Answers[Model.CurrentQuestionNumber];
            }
        }

        public void SetFirstQuestion()
        {
            //sets values for the first question
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
            QuizResults results = new QuizResults(this.quizID,this.username,Model.QuizType,Model.Questions,Model.UserInputs);

            if (Model.QuizType == "Instant")
            {
                results = AddInstantQuizValues(results);
            }

            //Converts the results object into JSON
            ServerRequest request = new ServerRequest();
            string resultsJSON = request.Serialise(results);

            //Creates a server connection to submit the quiz results
            ServerConnection server = new ServerConnection();
            server.ServerRequest("SUBMITRESULTS",new string[4] { this.quizID.ToString(), this.username, resultsJSON, results.correctTotal.ToString() });

            //Go to the quiz submit screen and pass the QuizResults object
            SubmitPage(results);
        }

        //Gets the multiple choice options from the questions and formats the question without having the choices in it
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

            //Removes the choices from the question for displaying
            int removeCount = (choiceEnd - choiceStart)+1;
            Model.CurrentQuestion = Model.CurrentQuestion.Remove(choiceStart, removeCount);
        }

        void SubmitPage(QuizResults results)
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "QuizSubmit";
            changePage.results = results;
            this.eventAggregator.Publish(changePage);
        }

        public void Quit()
        {
            //asks the user if they want to quit with Yes/No buttons
            MessageBoxResult result = MessageBox.Show("Are you sure you want to quit?","Quit quiz",MessageBoxButton.YesNo);
            //if the user wants to quit, then go back to the respective main menu
            if(result.ToString() == "Yes")
            {
                Debug.WriteLine(this.userType);
                if (this.userType == "TEACHER")
                {
                    ChangePageEvent changePage = new ChangePageEvent();
                    changePage.pageName = "TeacherMainMenu";
                    this.eventAggregator.Publish(changePage);
                }
                else
                {
                    ChangePageEvent changePage = new ChangePageEvent();
                    changePage.pageName = "StudentMainMenu";
                    this.eventAggregator.Publish(changePage);
                }
            }
        }

        QuizResults AddInstantQuizValues(QuizResults results)
        {
            int pointer = 0;

            //Adds the correct answers together and makes the "correct" list for easier displaying for teachers
            foreach (string answer in Model.Answers)
            {
                //String input or multiple choice
                if (Model.QuestionTypes[pointer] == 0 || Model.QuestionTypes[pointer] == 2)
                {
                    //If it's correct
                    if (Model.UserInputs[pointer] == Model.Answers[pointer])
                    {
                        results.correctTotal += 1;
                        results.correct.Add(true);
                    }
                    else
                    {
                        results.correct.Add(false);
                    }
                }
                //Integer input
                else if (Model.QuestionTypes[pointer] == 1)
                {
                    //If it's correct
                    if (Int32.Parse(Model.UserInputs[pointer]) == Int32.Parse(Model.Answers[pointer]))
                    {
                        results.correctTotal += 1;
                        results.correct.Add(true);
                    }
                    else
                    {
                        results.correct.Add(false);
                    }

                }

                pointer += 1;
            }
            return results;
        }
    }
}
