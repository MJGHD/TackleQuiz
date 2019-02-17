# Tackle

## KNOWN BUGS
just crashes when a network error occurs - Networking.cs needs threads

## TO DO IMMEDIATELY
Make QuizResults screen actually functional <br />
Request opening quizzes to the server - return QuizID and QuizContent. QuizContent's JSON will then be passed to the QuizScreenViewModel

## OBJECTIVES LEFT TO DO

2.	a) If a teacher has logged in, they should be shown a main menu with the options to create a new quiz; open a draft quiz; view the quizzes that they have 
currently set to their classes as well as their quizzes set history; the quizzes they have yet to mark, view and amend the classes they’ve set up and to view 
all public quizzes.
b) If a student has logged in, then they should be shown a main menu with the options to view currently set quizzes; review their quiz history; request to join 
a class and to view all public quizzes.
3.	a) A teacher should be able to create a quiz with as many questions as they’d like, custom timing, whether it’s an instant result quiz or not, different 
types of answering and custom questions.
b) Once the teacher has created a quiz, they should be able to send it to their students and/or be given the option to upload it to the public database of 
quizzes.
c) If a teacher hasn’t finished a quiz, they should be given the option to save as a draft
4.	The teacher should be able to open their draft quizzes, and then edit them or delete them.
5.
b) The user can quit at any point during the quiz with the “Quit” button, where they’re given a dialog asking whether they’re sure they want to quit or not.
c) When the user has pressed “Next” on the final question, they should be given a dialog asking if they are sure that they’re finished, and if they press “Yes”, 
then if it’s an instant quiz, their mark is displayed with their position on the leaderboard, and their mark is sent to the teacher. If it’s not, they’re taken 
back to the menu that they came back from and their answers are sent to the teacher to mark.
6.	A student should be able to check if they have any homework that has been set by the teacher, and if they do, they should be able to take the quiz
7.	a) A student or teacher should be able to browse and search the database of public quizzes
b) When a student clicks on a public quiz, if it’s an instant quiz then they should be shown a UI with the leaderboard and a “Play” button, where pressing “Play”
 should take them to the quiz.
c) When a teacher clicks on a public quiz, they should be given the options to “Play”, “Edit” or “Send to class”. If they press play, then they should play the 
quiz normally, if they press “Edit” a copy of the quiz can be modified by the teacher as if it were their own to put into their drafts or to send out to their 
students, and if they press “Send to class”, they are given a dialog as to which class they wish to send it out to.
8.	a) A teacher should be able to view the quizzes with answers that they have yet to mark
	b) The teacher should be able to click on the quiz that they wish to mark the answers to and be given a list of the questions and the students’ answers. Once 
	they have gone through it, they should be able to give the student a mark, which is sent to the student.
9.	a) A teacher should see their set quiz history and currently set quizzes
b) A student should see their quiz history with the amount of marks they received for it, and the ability to retake any quizzes
c) A teacher when looking at their quiz history should be able to export the student results as a CSV
10.	a) A teacher should able to create classes (which’re set a unique ID by the teacher), as well as amend any classes that they have
b) A student should also be able to request to join a class with the unique ID given to the teacher
