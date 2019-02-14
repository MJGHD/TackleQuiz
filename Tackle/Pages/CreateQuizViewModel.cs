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

//TOOO: make it so that multiple choice saves properly, or at all

namespace Tackle.Pages
{
    class CreateQuizViewModel
    {
        public CreateQuizModel Model { get; set; }
        private IEventAggregator eventAggregator;

        public CreateQuizViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            this.eventAggregator = eventAggregator;
            this.Model = new CreateQuizModel();
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
            SaveQuiz();
            //this.eventAggregator.Publish();
        }

        void SaveQuiz()
        {
            Directory.CreateDirectory(string.Format(@"{0}\zip", Environment.CurrentDirectory));
            SaveQuestions();
            SaveAnswers();
            SaveMetadata();
            SaveQuestionTypes();
            ZipFiles();
        }

        void SaveQuestions()
        {
            string directory = string.Format(@"{0}\zip\questions.txt", Environment.CurrentDirectory);
            using (File.Create(directory)) ;
            File.WriteAllLines(directory, this.Model.Questions.ToArray());
        }

        void SaveAnswers()
        {
            string directory = string.Format(@"{0}\zip\answers.txt", Environment.CurrentDirectory);
            using (File.Create(directory)) ;
            File.WriteAllLines(directory, this.Model.Answers.ToArray());
        }

        void SaveMetadata()
        {
            Debug.Write("a");
        }

        void SaveQuestionTypes()
        {
            string directory = string.Format(@"{0}\zip\questiontypes.txt", Environment.CurrentDirectory);
            using (File.Create(directory)) ;

            //Converting the integer question types to the strings that the file is expecting, e.g. "IntegerInput" instead of 1
            string[] lines = new string[this.Model.QuestionTypes.Count];

            int counter = 0;
            foreach(int type in this.Model.QuestionTypes)
            {
                switch (type)
                {
                    case 0:
                        lines[counter] = "StringInput";
                        break;
                    case 1:
                        lines[counter] = "IntegerInput";
                        break;
                    case 2:
                        lines[counter] = "MultipleChoice";
                        break;
                }
                counter += 1;
            }
            File.WriteAllLines(directory, lines);
        }

        void ZipFiles()
        {
            string startDirectory = String.Format(@"{0}\zip", Environment.CurrentDirectory);
            string zipDirectory = String.Format(@"{0}\{1}.zip",Environment.CurrentDirectory,this.Model.QuizID);
            ZipFile.CreateFromDirectory(startDirectory, zipDirectory);
        }
    }
}
