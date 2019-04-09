using Networking;
using Results;
using Stylet;
using Newtonsoft.Json;

namespace Tackle.Pages
{
    class QuizSubmitViewModel
    {
        private IEventAggregator eventAggregator;
        QuizResults results;
        public QuizSubmitModel Model { get; set; }

        public QuizSubmitViewModel(IEventAggregator eventAggregator, QuizResults results)
        {
            this.eventAggregator = eventAggregator;
            this.results = results;
            this.Model = new QuizSubmitModel();
            this.Model.QuestionTotal = results.questions.Length.ToString();
            this.Model.CorrectTotal = results.correctTotal.ToString();
            GetLeaderboard();
        }

        void GetLeaderboard()
        {
            //gets the Leaderboards object from the server
            ServerConnection server = new ServerConnection();
            string JSON = server.ServerRequest("GETLEADERBOARD", new string[] { this.results.quizID.ToString() });
            this.Model.Leaderboard = JsonConvert.DeserializeObject<Leaderboards>(JSON);

            int counter = 0;

            //iterates through the server response to add each leaderboard entry to the BindableCollection in the Model to be displayed in the view
            foreach(string username in this.Model.Leaderboard.usernames)
            {
                string correctNo = this.Model.Leaderboard.correct[counter];
                this.Model.LeaderboardEntries.Add(new LeaderboardEntry() { username = username, correctTotal = correctNo });
                counter += 1;
            }
        }
    }
}
