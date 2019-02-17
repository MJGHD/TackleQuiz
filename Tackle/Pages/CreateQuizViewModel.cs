using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Stylet;
using EventAggr;
using System.IO;
using System.IO.Compression;
using Networking;
using JSON;
using Quiz;

//TOOO: make it so that at the end, the multiple choice questions get the options appended to them (in the finish quiz function)

namespace Tackle.Pages
{
    class CreateQuizViewModel
    {
        public CreateQuizModel Model { get; set; }
        private IEventAggregator eventAggregator;

        public CreateQuizViewModel(IEventAggregator eventAggregator, string username)
        {
            this.eventAggregator = eventAggregator;
            this.Model = new CreateQuizModel();

            this.Model.Username = username;

            this.Model.CurrentQuestionNumber = 0;
            this.Model.NextButtonText = "Add question";
            this.Model.QuestionNumberDisplay = "Question 1/1";
        }

        public void NextQuestion()
        {
            SaveQuestion();
            this.Model.CurrentQuestionNumber += 1;
            SetUpQuestion();
        }

        public void PreviousQuestion()
        {
            //Goes back if it isn't the first question
            if (this.Model.CurrentQuestionNumber != 0)
            {
                this.Model.CurrentQuestionNumber -= 1;
                SetUpQuestion();
            }
        }

        void SetUpQuestion()
        {
            //Add blank question, answer and question type to the question, answer and question types lists
            if (this.Model.CurrentQuestionNumber == this.Model.Questions.Count)
            {
                this.Model.Questions.Add("");
                this.Model.Answers.Add("");
                this.Model.QuestionTypes.Add(0);
                this.Model.AllMultipleChoiceInputs.Add("");
            }

            this.Model.CurrentQuestion = this.Model.Questions[this.Model.CurrentQuestionNumber];
            this.Model.CurrentQuestionAnswer = this.Model.Answers[this.Model.CurrentQuestionNumber];

            this.Model.QuestionNumberDisplay = $"Question {this.Model.CurrentQuestionNumber + 1}/{this.Model.Questions.Count}";

            this.Model.CurrentQuestionType = this.Model.QuestionTypes[this.Model.CurrentQuestionNumber];

            this.Model.MultipleChoiceInputs = this.Model.AllMultipleChoiceInputs[this.Model.CurrentQuestionNumber];

            //Set up next button text
            if (this.Model.CurrentQuestionNumber == this.Model.Questions.Count - 1)
            {
                this.Model.NextButtonText = "Add question";
            }
            else
            {
                this.Model.NextButtonText = "Next question";
            }
        }

        public void DeleteQuestion()
        {
            //If it's not the only page, delete
            if(this.Model.Questions.Count > 1){
                this.Model.Answers.RemoveAt(this.Model.CurrentQuestionNumber);
                this.Model.Questions.RemoveAt(this.Model.CurrentQuestionNumber);
                this.Model.AllMultipleChoiceInputs.RemoveAt(this.Model.CurrentQuestionNumber);

                //If deleting the first question, stay at the current question number
                if(this.Model.CurrentQuestionNumber != 0)
                {
                    this.Model.CurrentQuestionNumber -= 1;
                }
                SetUpQuestion();
            }
            
        }

        public void ShowSettings()
        {
            this.Model.tempQuestionType = this.Model.CurrentQuestionType;
            this.Model.CurrentQuestionType = 3;
        }

        public void QuitSettings()
        {
            this.Model.CurrentQuestionType = this.Model.tempQuestionType;
        }

        void SaveQuestion()
        {
            if (this.Model.CurrentQuestionNumber == this.Model.Questions.Count)
            {
                AddQuestion();
            }
            else
            {
                this.Model.Questions[this.Model.CurrentQuestionNumber] = this.Model.CurrentQuestion;
                this.Model.Answers[this.Model.CurrentQuestionNumber] = this.Model.CurrentQuestionAnswer;
                this.Model.QuestionTypes[this.Model.CurrentQuestionNumber] = this.Model.CurrentQuestionType;
                this.Model.AllMultipleChoiceInputs[this.Model.CurrentQuestionNumber] = this.Model.MultipleChoiceInputs;
            }
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

        //Adds the question and answer inputted by the teacher to the Questions and Answers list
        void AddQuestion()
        {
            this.Model.Questions.Add(this.Model.CurrentQuestion);
            this.Model.Answers.Add(this.Model.CurrentQuestionAnswer);
            this.Model.AllMultipleChoiceInputs.Add(this.Model.MultipleChoiceInputs);
            this.Model.QuestionTypes.Add(this.Model.CurrentQuestionType);
        }

        public void FinishQuiz()
        {
            string success = SaveQuiz();

            if(success == "success")
            {
                MessageBox.Show("Quiz submission successful");
                this.eventAggregator.Publish("TeacherMainMenu");
            }
            else
            {
                MessageBox.Show("Quiz submission unsuccessful");
            }
        }

        string SaveQuiz()
        {
            //save last question
            SaveQuestion();

            //Put the multiple choice options into the question titles, so that they appear when taking the quiz
            MultiChoiceNormalisation();

            //Creates instance of Quiz class to be serialised into JSON
            Quiz.Quiz quiz = MakeQuiz();

            //Submit to server
            string success = Submit(quiz);

            return success;
        }

        void MultiChoiceNormalisation()
        {
            int counter = 0;

            foreach(int questionType in this.Model.QuestionTypes)
            {
                //if it's multiple choice
                if(questionType == 2)
                {
                    //Add to the end of the question in the normal format of multi choice - {option1,option2}
                    this.Model.Questions[counter] += " {"+this.Model.Answers[counter]+"}";
                }
                counter += 1;
            }
        }

        string Submit(Quiz.Quiz quiz)
        {
            ServerRequest serverRequest = new ServerRequest();
            string quizJSON = serverRequest.Serialise(quiz);

            ServerConnection server = new ServerConnection();
            string success = server.ServerRequest("CREATEQUIZ", new string[] {this.Model.Username,this.Model.QuizType,this.Model.QuizTitle,quizJSON });

            return success;
        }

        //Fills up Quiz instance's fields
        Quiz.Quiz MakeQuiz()
        {
            Quiz.Quiz quiz = new Quiz.Quiz();

            quiz.Answers = this.Model.Answers.ToArray();
            quiz.Questions = this.Model.Questions.ToArray();
            quiz.QuestionTypes = this.Model.QuestionTypes.ToArray();
            quiz.TimeAllocated = this.Model.TimeAllocated;
            quiz.QuizType = this.Model.QuizType;

            return quiz;
        }
    }
}
