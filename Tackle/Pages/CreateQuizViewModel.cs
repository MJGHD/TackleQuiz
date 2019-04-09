using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Stylet;
using EventAggr;
using Networking;
using JSON;
using Quiz;
using System.Collections.Generic;

namespace Tackle.Pages
{
    class CreateQuizViewModel : IHandle<string>
    {
        public CreateQuizModel Model { get; set; }
        private IEventAggregator eventAggregator;
        private IWindowManager windowManager;

        //Used only when sending a quiz to a class
        string classID;

        public CreateQuizViewModel(IEventAggregator eventAggregator, string username, IWindowManager windowManager, int quizID)
        {
            this.eventAggregator = eventAggregator;
            //subscribe to the event aggregator for sending the quiz to a class at the end
            eventAggregator.Subscribe(this);

            //sets everything up as it should be and assigns the default values for editing a quiz from the first question
            this.windowManager = windowManager;

            this.Model = new CreateQuizModel();

            this.Model.Username = username;

            this.Model.CurrentQuestionNumber = 0;
            this.Model.NextButtonText = "Add question";
            this.Model.Public = true;
            
            // quiz ID being -1 means that it's a new quiz, if the quiz ID put into the ViewModel as a parameter isn't -1 then load the quiz contents from the server
            if (quizID != -1)
            {
                string JSON = QuizHandling.GetQuizJSON(quizID);
                //temporary to return values into
                string[] questions;
                int[] questionTypes;
                string[] answers;
                (questions, questionTypes, answers, Model.TimeAllocated, Model.QuizType) = QuizHandling.OpenQuiz(JSON);
                FillModel(questions,questionTypes,answers);
                SetUpQuestion();
            }
            else
            {
                //Sets the default values for quiz entry
                this.Model.QuestionNumberDisplay = "Question 1/1";
                this.Model.Instant = true;
                this.Model.QuizType = "Instant";
                //start on settings
                this.Model.CurrentQuestionType = 3;
            }
        }

        public void NextQuestion()
        {
            //Limits the amount of questions to 50
            if (this.Model.CurrentQuestionNumber != 49)
            {
                SaveQuestion();
                this.Model.CurrentQuestionNumber += 1;
                SetUpQuestion();
            }
        }

        public void PreviousQuestion()
        {
            SaveQuestion();

            //Goes back if it isn't the first question
            if (this.Model.CurrentQuestionNumber != 0)
            {
                this.Model.CurrentQuestionNumber -= 1;
                SetUpQuestion();
            }
        }

        //Sets up the model's properties to contain the content of that question
        void SetUpQuestion()
        {
            //Add blank question, answer and question type to the question, answer and question types lists
            if (this.Model.CurrentQuestionNumber == this.Model.Questions.Count)
            {
                this.Model.Questions.Add("");
                this.Model.Answers.Add("");
                this.Model.QuestionTypes.Add(0);
                this.Model.AllMultipleChoiceInputs.Add("");
                this.Model.MultiChoiceAnswers.Add("");
            }

            //assigns the values of the question to the model's properties
            this.Model.CurrentQuestionType = this.Model.QuestionTypes[this.Model.CurrentQuestionNumber];

            this.Model.CurrentQuestion = this.Model.Questions[this.Model.CurrentQuestionNumber];
            this.Model.CurrentQuestionAnswer = this.Model.Answers[this.Model.CurrentQuestionNumber];

            this.Model.QuestionNumberDisplay = $"Question {this.Model.CurrentQuestionNumber + 1}/{this.Model.Questions.Count}";

            this.Model.MultipleChoiceInputs = this.Model.AllMultipleChoiceInputs[this.Model.CurrentQuestionNumber];
            this.Model.MultipleChoiceAnswer = this.Model.MultiChoiceAnswers[this.Model.CurrentQuestionNumber];

            //Set up next button text - if the current question is the last one, then the program will add a question so "Add Question" will be displayed instead of "Next Question"
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
            //If it's not the only page, delete - this means that you can't have a totally blank quiz with 0 questions
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
            Debug.WriteLine(this.Model.TimeAllocated);
            //checks if the title and time allowed are empty
            if (this.Model.QuizTitle != "" && this.Model.QuizTitle != null && this.Model.TimeAllocated != 0)
            {
                this.Model.CurrentQuestionType = this.Model.tempQuestionType;
            }
            else
            {
                MessageBox.Show("The title and time allocated must be filled in");
            }
        }

        void SaveQuestion()
        {
            //checks whether the settings are open or not - if they are, save as the temporary question type
            if (this.Model.CurrentQuestionType == 3)
            {
                this.Model.CurrentQuestionType = this.Model.tempQuestionType;
            }

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
                this.Model.MultiChoiceAnswers[this.Model.CurrentQuestionNumber] = this.Model.MultipleChoiceAnswer;
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

            //If the multiple choice answer field isn't null, then add the inputted answer. If it is, then add a blank string
            if (!(this.Model.MultipleChoiceAnswer is null))
            {
                this.Model.MultiChoiceAnswers.Add(this.Model.MultipleChoiceAnswer);
            }
            else
            {
                this.Model.MultiChoiceAnswers.Add("");
            }
            
        }

        public void FinishQuiz()
        {
            //If the quiz is public, then just submit, otherwise give the teacher options to save as draft or send to a class
            if (this.Model.Public)
            {
                string success = SaveQuiz();

                if (success == "success")
                {
                    MessageBox.Show("Quiz submission successful");
                    TeacherMainMenu();
                    this.eventAggregator.Publish("TeacherMainMenu");

                }
                else
                {
                    MessageBox.Show("Quiz submission unsuccessful");
                }
            }
            else
            {
                //if the user wants to send it to their class, then they're given a dialog box with a class ID to send it to
                MessageBoxResult result = MessageBox.Show("Send to a class?", "Send to class", MessageBoxButton.YesNo);
                if(result.ToString() == "Yes")
                {
                    var classIDViewModel = new ClassSendViewModel(this.eventAggregator);
                    this.windowManager.ShowDialog(classIDViewModel);
                    SubmitToClass(this.classID);
                }

                //If the user wants to save as a draft, then they are able to
                result = MessageBox.Show("Save as draft?", "Save draft", MessageBoxButton.YesNo);

                if (result.ToString() == "Yes")
                {
                    string success = SaveDraft();
                    if(success == "success")
                    {
                        MessageBox.Show("Draft saved successfully");
                        TeacherMainMenu();
                    }
                    else
                    {
                        MessageBox.Show("Draft saved unsuccessfully");
                    }
                }
            }
        }

        //Handles string event published from the class entry dialog box
        public void Handle(string classID)
        {
            this.classID = classID;
        }

        public void Quit()
        {
            MessageBoxResult result = MessageBox.Show("Save as draft?","Save",MessageBoxButton.YesNoCancel);
            if(result.ToString() == "Yes")
            {
                string success = SaveDraft();

                if(success == "success")
                {
                    MessageBox.Show("Draft creation successful");
                    TeacherMainMenu();
                }
                else
                {
                    MessageBox.Show("Draft creation unsuccessful");
                }
            }
            else if(result.ToString() == "No")
            {
                TeacherMainMenu();
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

        string SaveDraft()
        {
            SaveQuestion();

            MultiChoiceNormalisation();

            Quiz.Quiz quiz = MakeQuiz();

            string success = DraftSubmit(quiz);

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

            //Saves the expected answer for the multiple choice questions as the normal answer
            for(int i = 0; i < this.Model.Questions.Count; i++)
            {
                if(this.Model.MultiChoiceAnswers[i] != "")
                {
                    this.Model.Answers[i] = this.Model.MultiChoiceAnswers[i];
                }
            }
        }

        //Submits the finished public quiz to the server
        string Submit(Quiz.Quiz quiz)
        {
            ServerRequest serverRequest = new ServerRequest();
            string quizJSON = serverRequest.Serialise(quiz);

            ServerConnection server = new ServerConnection();
            string success = server.ServerRequest("CREATEQUIZ", new string[] {this.Model.Username,this.Model.QuizType,this.Model.QuizTitle,quizJSON,this.Model.Public.ToString() });

            //Removes UTF-8 encoding's annoying "\0" character for whitespaece
            success = success.Replace("\0", string.Empty);

            return success;
        }

        string SubmitToClass(string classID)
        {
            SaveQuestion();
            Quiz.Quiz quiz = MakeQuiz();

            ServerRequest serverRequest = new ServerRequest();
            string quizJSON = serverRequest.Serialise(quiz);

            ServerConnection server = new ServerConnection();
            string success = server.ServerRequest("SUBMITQUIZTOCLASS", new string[] { this.Model.Username, this.Model.QuizType, this.Model.QuizTitle, quizJSON, classID });

            //Removes UTF-8 encoding's annoying "\0" character for whitespaece
            success = success.Replace("\0", string.Empty);

            return success;
        }

        //Submits a draft quiz
        string DraftSubmit(Quiz.Quiz quiz)
        {
            ServerRequest serverRequest = new ServerRequest();
            string quizJSON = serverRequest.Serialise(quiz);

            ServerConnection server = new ServerConnection();
            string success = server.ServerRequest("CREATEDRAFT", new string[] { this.Model.Username, this.Model.QuizType, this.Model.QuizTitle, quizJSON});

            //Removes UTF-8 encoding's annoying "\0" character for whitespaece
            success = success.Replace("\0", string.Empty);

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

        void TeacherMainMenu()
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "TeacherMainMenu";
            this.eventAggregator.Publish(changePage);
        }

        public void CheckBoxChangeState(string type)
        {
            Debug.WriteLine(type);
            if (type == "Instant")
            {
                if (this.Model.Instant is true)
                {
                    this.Model.QuizType = "Instant";
                    this.Model.Instant = true;
                }
                else
                {
                    this.Model.QuizType = "NotInstant";
                    this.Model.Instant = false;
                }
            }
            else
            {
                if (this.Model.Public is true)
                {
                    this.Model.Public = true;
                }
                else
                {
                    this.Model.Public = false;
                }
            }
        }

        //fills model when editing quiz
        void FillModel(string[] questions, int[] questionTypes, string[] answers)
        {
            int counter = 0;

            foreach(string question in questions)
            {
                //if it's a multiple choice question, add the appropriate values
                if (questionTypes[counter] == 2)
                {
                    string tempQuestion;
                    string multiChoice;
                    (multiChoice, tempQuestion) = GetMultipleChoices(question);
                    this.Model.AllMultipleChoiceInputs.Add("");
                    this.Model.MultiChoiceAnswers.Add(answers[counter]);
                    this.Model.Questions.Add(tempQuestion);
                    this.Model.Answers.Add(multiChoice);
                }
                else
                {
                    this.Model.AllMultipleChoiceInputs.Add("");
                    this.Model.MultiChoiceAnswers.Add("");
                    this.Model.Questions.Add(question);
                    this.Model.Answers.Add(answers[counter]);
                }

                this.Model.QuestionTypes.Add(questionTypes[counter]);

                counter += 1;
            }
        }

        //Gets the multiple choice options from the questions
        (string, string) GetMultipleChoices(string question)
        {
            List<string> choices = new List<string> { };

            //Gets the indexes of the beginning and end of the choices
            int choiceStart = question.IndexOf('{');
            int choiceEnd = question.IndexOf('}');

            //Removes the start and end of the end of the choices and then adds the choices to the options array
            string choiceString = question.Remove(0, choiceStart + 1);
            choiceString = choiceString.TrimEnd('}');

            //Removes the choices from the question for displaying
            int removeCount = (choiceEnd - choiceStart) + 1;
            question = question.Remove(choiceStart, removeCount);

            return (choiceString, question);
        }
    }
}
