namespace vs_project.Model
{
    public class submission : assignment
    {
        public int submissionID { get; set; }//this is the counter in student_assignments table
        public int studentID { get; set; }
        public string submissionPath { get; set; } = string.Empty;

    }
}
