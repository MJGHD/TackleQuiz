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
            //if the teacher wants to take a quiz, then pass the quiz ID to the QuizScreenViewModel
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "TakeQuiz";
            changePage.quizID = Int32.Parse(quizID);
            this.eventAggregator.Publish(changePage);
            this.RequestClose(true);
        }

        public void Edit()
        {
            //puts the quiz ID into the quiz edit page so that the teacher can edit the quiz as if it's their own
            ChangePageEvent changePage = new ChangePageEvent();
            changePage.pageName = "EditQuiz";
            changePage.quizID = Int32.Parse(quizID);
            this.eventAggregator.Publish(changePage);
            this.RequestClose(true);
        }

        public void SendToClass()
        {
            //shows the send to class dialog pop up, which returns the quiz ID
            var classIDViewModel = new ClassSendViewModel(this.eventAggregator);
            this.windowManager.ShowDialog(classIDViewModel);

            // used in testing to make sure that the correct quiz ID was being returned
            //Debug.WriteLine(this.quizID);

            //sends the quiz to the class
            ServerConnection server = new ServerConnection();
            string success = server.ServerRequest("SENDTOCLASS", new string[] { this.username, this.classID, this.quizID });

            //Removes UTF-8 encoding's annoying "\0" character for whitespaece
            success = success.Replace("\0", string.Empty);

            //shows whether sending to the class was a success
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
            //if it's a deletable quiz (not public), then send a server request to delete it
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
