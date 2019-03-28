using Results;
using Stylet;

namespace Tackle.Pages
{
    class QuizSubmitViewModel
    {
        private IEventAggregator eventAggregator;
        QuizResults results;

        public QuizSubmitViewModel(IEventAggregator eventAggregator, QuizResults results)
        {
            this.eventAggregator = eventAggregator;
            this.results = results;
        }
    }
}
