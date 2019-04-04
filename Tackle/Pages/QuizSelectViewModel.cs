using Networking;
using System.Windows;
using Stylet;
using EventAggr;
using System;
using System.Diagnostics;

namespace Tackle.Pages
{
    class QuizSelectViewModel : Screen, IHandle<string>
    {
        IWindowManager windowManager;
        IEventAggregator eventAggregator;

        bool deletable;
        string classID;
        string quizID;
        string username;

        public QuizSelectViewModel(string quizID, string username, bool deletable, IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            this.deletable = deletable;
            this.quizID = quizID;
            this.username = username;
        }

        public void TakeQuiz()
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "TakeQuiz";
            changePage.quizID = Int32.Parse(quizID);
            this.eventAggregator.Publish(changePage);
            this.RequestClose(true);
        }

        public void Edit()
        {
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "EditQuiz";
            changePage.quizID = Int32.Parse(quizID);
            this.eventAggregator.Publish(changePage);
            this.RequestClose(true);
        }

        public void SendToClass()
        {
            var classIDViewModel = new ClassSendViewModel(this.eventAggregator);
            this.windowManager.ShowDialog(classIDViewModel);

            Debug.WriteLine(this.quizID);

            ServerConnection server = new ServerConnection();
            string success = server.ServerRequest("SENDTOCLASS", new string[] { this.username, this.classID, this.quizID });

            //Removes UTF-8 encoding's annoying "\0" character for whitespaece
            success = success.Replace("\0", string.Empty);

            if(success == "success")
            {
                MessageBox.Show("Successfully sent quiz to class");
            }
            else
            {
                MessageBox.Show("Quiz has not been sent to the class");
            }
        }

        public void Delete()
        {
            if (this.deletable)
            {
                ServerConnection server = new ServerConnection();
                string success = server.ServerRequest("DELETEDRAFT", new string[] { this.quizID });

                //Removes UTF-8 encoding's annoying "\0" character for whitespaece
                success = success.Replace("\0", string.Empty);

                if (success == "success")
                {
                    MessageBox.Show("Successfully deleted quiz");
                }
                else
                {
                    MessageBox.Show("Quiz has not been deleted");
                }
            }
            else
            {
                MessageBox.Show("This quiz is not deletable");
            }
        }

        //Handles the string event raised by the ClassSendViewModel
        public void Handle(string classID)
        {
            this.classID = classID;
        }
    }
}
