using Stylet;

namespace Tackle.Pages
{
    class ClassSendViewModel : Screen
    {
        public ClassSendModel Model { get; set; }
        IEventAggregator eventAggregator;

        public ClassSendViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.Model = new ClassSendModel();
        }

        public void Close()
        {
            this.eventAggregator.Publish(this.Model.ClassID);
            this.RequestClose(true);
        }
    }
}
