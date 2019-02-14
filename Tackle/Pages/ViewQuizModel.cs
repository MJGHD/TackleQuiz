using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

namespace Tackle.Pages
{
    class ViewQuizModel : PropertyChangedBase
    {
        private string[] _questions;
        private string _quizType;
        private string _username;
        private bool[] _correctQuestions;
        private string[] _answers;
        private int _quizID;

        private string _currentQuestion;
        private int _currentQuestionNumber;
        private string _currentUserAnswer;
        private bool _currentQuestionCorrect;
        private string _nextButtonText;

        public string[] Questions
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
        public bool[] CorrectQuestions
        {
            get { return this._correctQuestions; }
            set { SetAndNotify(ref this._correctQuestions, value); }
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
        public int CurrentQuestionNumber
        {
            get { return this._currentQuestionNumber; }
            set { SetAndNotify(ref this._currentQuestionNumber, value); }
        }
        public string CurrentUserAnswer
        {
            get { return this._currentUserAnswer; }
            set { SetAndNotify(ref this._currentUserAnswer, value); }
        }
        public bool CurrentQuestionCorrect
        {
            get { return this._currentQuestionCorrect; }
            set { SetAndNotify(ref this._currentQuestionCorrect, value); }
        }
        public string[] Answers
        {
            get { return this._answers; }
            set { SetAndNotify(ref this._answers,value); }
        }
        public string NextButtonText
        {
            get { return _nextButtonText; }
            set { SetAndNotify(ref this._nextButtonText, value); }
        }

    }
}
