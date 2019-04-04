using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

namespace Tackle.Pages
{
    class ManageClassPopUpModel : PropertyChangedBase
    {
        public ManageClassPopUpModel()
        {
            this.StudentList = new BindableCollection<Student>();
        }

        private BindableCollection<Student> _studentList;

        public BindableCollection<Student> StudentList
        {
            get { return this._studentList; }
            set { SetAndNotify(ref this._studentList, value); }
        }
    }

    //class for the BindableCollection to bind to
    public class Student
    {
        public Student(string username)
        {
            this.username = username;
        }

        public string username { get; set; }
    }
}