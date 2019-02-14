using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

namespace Tackle.Pages
{
    class CreateQuizModel : PropertyChangedBase
    {
        private List<string> _questions;
        private string _quizType;
        private string _username;
        private List<string> _answers;
        private int _quizID;

        private string _currentQuestion;
        private string _currentQuestionAnswer;
        private int _currentQuestionNumber;
        private string _questionNumberDisplay;
        private int _currentQuestionType;
        private string _currentQuestionTypeText;
        private string _nextButtonText;
        private List<int> _questionTypes;
        private string _multipleChoiceInputs;
        private List<string> _allMultipleChoiceInputs;

        public List<string> Questions
        {
            get { return this._questions; }
            set { SetAndNotify(ref this._questions, value); }
        }
        public string QuizType
        {
            get { return this._quizType; }
            set { SetAndNotify(ref this._quizType, value); }
        }
        public string Username
        {
            get { return this._username; }
            set { SetAndNotify(ref this._username, value); }
        }
        public List<string> Answers
        {
            get { return this._answers; }
            set { SetAndNotify(ref this._answers, value); }
        }
        public int QuizID
        {
            get { return this._quizID; }
            set { SetAndNotify(ref this._quizID, value); }
        }
        public string CurrentQuestion
        {
            get { return this._currentQuestion; }
            set { SetAndNotify(ref this._currentQuestion, value); }
        }
        public string CurrentQuestionAnswer
        {
            get { return this._currentQuestionAnswer; }
            set { SetAndNotify(ref this._currentQuestionAnswer, value); }
        }
        public int CurrentQuestionNumber
        {
            get { return this._currentQuestionNumber; }
            set { SetAndNotify(ref this._currentQuestionNumber, value); }
        }
        public string QuestionNumberDisplay
        {
            get { return this._questionNumberDisplay; }
            set { SetAndNotify(ref this._questionNumberDisplay, value); }
        }
        public int CurrentQuestionType
        {
            get { return this._currentQuestionType; }
            set {
                SetAndNotify(ref this._currentQuestionType, value);
                //Changes the CurrentQuestionTypeText value
                switch (CurrentQuestionType) {
                    case 0:
                        CurrentQuestionTypeText = "Text";
                        break;
                    case 1:
                        CurrentQuestionTypeText = "Integer";
                        break;
                    case 2:
                        CurrentQuestionTypeText = "Multiple Choice";
                        break;
                };
            }
        }
        public string CurrentQuestionTypeText
        {
            get { return this._currentQuestionTypeText; }
            set { SetAndNotify(ref this._currentQuestionTypeText, value); }
        }
        public string NextButtonText
        {
            get { return this._nextButtonText; }
            set { SetAndNotify(ref this._nextButtonText, value); }
        }
        public List<int> QuestionTypes
        {
            get { return this._questionTypes; }
            set { SetAndNotify(ref this._questionTypes, value); }
        }
        public string MultipleChoiceInputs
        {
            get { return this._multipleChoiceInputs; }
            set { SetAndNotify(ref this._multipleChoiceInputs, value); }
        }
        public List<string> AllMultipleChoiceInputs
        {
            get { return this._allMultipleChoiceInputs; }
            set { SetAndNotify(ref this._allMultipleChoiceInputs, value); }
        }

        public string[] TypesOfQuestion { get; set; } = new string[3] { "Text", "Integer", "Multiple Choice" };

        public CreateQuizModel()
        {
            //Initalises lists and BindableCollection so they aren't null
            this.Questions = new List<string>();
            this.Answers = new List<string>();
            this.QuestionTypes = new List<int>();
            this.AllMultipleChoiceInputs = new List<string>();
        }
    }
}
