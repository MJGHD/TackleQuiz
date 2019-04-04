using Stylet;
using System.Collections.Generic;

namespace Tackle.Pages
{
    class QuizSubmitModel : PropertyChangedBase
    {

        //All of the quiz attempts that are included in the teacher's list of quizzes
        private List<LeaderboardEntry> _leaderboardEntries;

        public List<LeaderboardEntry> LeaderboardEntries
        {
            get { return this._leaderboardEntries; }
            set { SetAndNotify(ref this._leaderboardEntries, value); }
        }

        private Leaderboards _leaderboard;

        public Leaderboards Leaderboard
        {
            get { return this._leaderboard; }
            set { SetAndNotify(ref this._leaderboard, value); }
        }

        private string _correctTotal;
        private string _questionTotal;

        public string CorrectTotal
        {
            get { return this._correctTotal; }
            set { SetAndNotify(ref this._correctTotal, value); }
        }

        public string QuestionTotal
        {
            get { return this._questionTotal; }
            set { SetAndNotify(ref this._questionTotal, value); }
        }

        public QuizSubmitModel()
        {
            this.LeaderboardEntries = new List<LeaderboardEntry>();
        }
    }

    class LeaderboardEntry
    {
        public string username { get; set; }
        public string correctTotal { get; set; }
    }

    public class Leaderboards
    {
        public string[] usernames;
        public string[] correct;
    }
}
