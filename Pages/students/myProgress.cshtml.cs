using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.OleDb;
using vs_project.App_Code;
using vs_project.Model;

namespace vs_project.Pages.students
{
    public class myProgressModel : PageModel
    {
        public List<assignment> unDone = new List<assignment>();
        public void OnGet()
        {
            string sessionUser = HttpContext.Session.GetString("Username");
            if (sessionUser == null)
            {
                Response.Redirect("/log in");
                return;
            }
            using (var con = new OleDbConnection(Imp_Data.ConString))
            {
                con.Open();
                int studentID = 0;
                string studentIdSql = "SELECT ID FROM students WHERE username = ?";
                using (var cmd = new OleDbCommand(studentIdSql, con))
                {
                    cmd.Parameters.AddWithValue("?", sessionUser);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        studentID = Convert.ToInt32(result);
                    }
                    else
                    {//if no student
                        return;
                    }
                }

                string sql = @"SELECT A.[ID], A.[Title], A.[subject] 
                       FROM [assignments] AS A 
                       INNER JOIN [student_assignments] AS SA ON A.[ID] = SA.[assignmentID] 
                       WHERE SA.[studentID] = ? AND SA.[isDone] = 0";
               

                using (var cmd = new OleDbCommand(sql, con))
                {
                    // This '?' maps to your session username
                    cmd.Parameters.AddWithValue("?", studentID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            unDone.Add(new assignment
                            {
                                ID = int.Parse(reader["ID"].ToString()),
                                Title = reader["Title"].ToString(),
                                Subject = reader["subject"].ToString()
                            });
                        }
                    }
                }
            }
        }
    }
}