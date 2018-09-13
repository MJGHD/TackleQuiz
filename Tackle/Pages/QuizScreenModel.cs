using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

namespace Tackle.Pages
{
    class QuizScreenModel : PropertyChangedBase
    {
        private string[] _questions;
        private string[] _questionTypes;
        private string[] _answers;
        private string _quizType;
        private int _timeLeft;
        private string _timeLeftDisplay;
        private int _currentQuestionNumber;
        private string _currentQuestion;
        private string _currentAnswer;
        private string _questionNumberDisplay;
        private int _currentQuestionType;
        private string _userInput;
        private string[] _userInputs;
        private string _nextButtonText;

        public string[] Questions
        {
            get { return this._questions; }
            set { SetAndNotify(ref this._questions, value); }
        }

        public string[] QuestionTypes
        {
            get { return this._questionTypes; }
            set { SetAndNotify(ref this._questionTypes, value); }
        }

        public string[] Answers
        {
            get { return this._answers; }
            set { SetAndNotify(ref this._answers, value); }
        }

        public string QuizType
        {
            get { return this._quizType; }
            set { SetAndNotify(ref this._quizType, value); }
        }

        public int TimeLeft
        {
            get { return this._timeLeft; }
            set { SetAndNotify(ref this._timeLeft, value); }
        }

        public string TimeLeftDisplay
        {
            get { return this._timeLeftDisplay; }
            set { SetAndNotify(ref this._timeLeftDisplay, value); }
        }

        public int CurrentQuestionNumber
        {
            get { return this._currentQuestionNumber; }
            set { SetAndNotify(ref this._currentQuestionNumber, value); }
        }

        public string CurrentQuestion
        {
            get { return this._currentQuestion; }
            set { SetAndNotify(ref this._currentQuestion, value); }
        }

        public string CurrentAnswer
        {
            get { return this._currentAnswer; }
            set { SetAndNotify(ref this._currentAnswer, value); }
        }

        public string QuestionNumberDisplay
        {
            get { return this._questionNumberDisplay; }
            set { SetAndNotify(ref this._questionNumberDisplay, value); }
        }

        public int CurrentQuestionType
        {
            get { return this._currentQuestionType; }
            set { SetAndNotify(ref this._currentQuestionType, value); }
        }

        public string UserInput
        {
            get { return this._userInput; }
            set { SetAndNotify(ref this._userInput, value); }
        }

        public string[] UserInputs
        {
            get { return this._userInputs; }
            set { SetAndNotify(ref this._userInputs, value); }
        }

        public string NextButtonText
        {
            get { return this._nextButtonText; }
            set { SetAndNotify(ref this._nextButtonText, value); }
        }

    }
}
