using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

namespace Tackle.Pages
{
    class CreateQuizModel : PropertyChangedBase
    {
        public int tempQuestionType;

        private List<string> _questions;
        private string _quizType;
        private string _username;
        private List<string> _answers;
        private List<string> _multiChoiceAnswers;
        private int _quizID;
        private string _quizTitle;

        private string _currentQuestion;
        private string _currentQuestionAnswer;
        private string _multipleChoiceAnswer;
        private int _currentQuestionNumber;
        private string _questionNumberDisplay;
        private int _currentQuestionType;
        private string _nextButtonText;
        private List<int> _questionTypes;
        private string _multipleChoiceInputs;
        private List<string> _allMultipleChoiceInputs;
        private int _timeAllocated;
        private bool _instant;
        private bool _public;

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
        public List<string> MultiChoiceAnswers
        {
            get { return this._multiChoiceAnswers; }
            set { SetAndNotify(ref this._multiChoiceAnswers, value); }
        }
        public int QuizID
        {
            get { return this._quizID; }
            set { SetAndNotify(ref this._quizID, value); }
        }
        public string QuizTitle
        {
            get { return this._quizTitle; }
            set { SetAndNotify(ref this._quizTitle, value); }
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
        public string MultipleChoiceAnswer
        {
            get { return this._multipleChoiceAnswer; }
            set { SetAndNotify(ref this._multipleChoiceAnswer, value); }
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
                //If the selected type has CHANGED, clear the current question answer (prevents forcing of text in integer questions, but ignores when settings is closed or opened)
                if(this._currentQuestionType != value && this._currentQuestionType != 3 && value != 3)
                {
                    this.CurrentQuestionAnswer = "";
                }
                SetAndNotify(ref this._currentQuestionType, value);
            }
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
        public int TimeAllocated
        {
            get { return this._timeAllocated; }
            set { SetAndNotify(ref this._timeAllocated, value); }
        }
        public bool Instant
        {
            get { return this._instant; }
            set { SetAndNotify(ref this._instant, value); }
        }
        public bool Public
        {
            get { return this._public; }
            set { SetAndNotify(ref this._public, value); }
        }

        public CreateQuizModel()
        {
            //Initalises lists and BindableCollection so they aren't null
            this.Questions = new List<string>();
            this.Answers = new List<string>();
            this.QuestionTypes = new List<int>();
            this.AllMultipleChoiceInputs = new List<string>();
            this.MultiChoiceAnswers = new List<string>();
        }
    }
}
