using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.OleDb;
using vs_project.App_Code;
using vs_project.Model;

namespace vs_project.Pages.students
{
    public class general_assignmentsModel : PageModel
    {
        public assignment CurrentAssignment { get; set; }

        public void OnGet(int id)
        {
            using (var conn = new OleDbConnection(Imp_Data.ConString))
            {
                conn.Open();
                // Select only the assignment the student clicked
                string sql = $"SELECT * FROM [assignments] WHERE ID = {id}";

                using (var cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CurrentAssignment = new assignment
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                FileName = reader["FileName"].ToString(),
                                Subject = reader["subject"].ToString()
                            };
                        }
                    }
                }
            }
        }
        public async Task<IActionResult> OnPost(int assignmentID, IFormFile upload)
        {
            if (upload == null || upload.Length == 0)
            {
                return Page();
            }
            string user = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(user))
            {
                return RedirectToPage("/log in");
            }
            string filename = "";
            string extension = Path.GetExtension(upload.FileName);
            string filepath = Path.Combine("wwwroot", "uploads", "submissions", upload.FileName);
            int c = 1;
            while (System.IO.File.Exists(filepath))
            {
                filename = $"{upload.FileName}_{c}{extension}";
                filepath = Path.Combine("wwwroot", "uploads", "submissions", filename);
                c++;
            }
            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await upload.CopyToAsync(stream);
            }
            using (var conn = new OleDbConnection(Imp_Data.ConString))
            {
                conn.Open();
                int studentID = 0;
                string studentIdSql = "SELECT [ID] FROM [students] WHERE [username] = ?";
                using(var cmd = new OleDbCommand(studentIdSql, conn))
                {
                    cmd.Parameters.AddWithValue("?", user);
                    studentID = (int)cmd.ExecuteScalar();
                }
                
                string sql = "UPDATE [student_assignments] SET [isDone] = True, [filePath] = ? WHERE [studentID] = ? AND [assignmentID] = ?";
                using (var cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("?", filepath);
                    cmd.Parameters.AddWithValue("?", studentID);
                    cmd.Parameters.AddWithValue("?", assignmentID);
                    int rowsaffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("rows " + rowsaffected + " studentid " + studentID + "assignmentid" + assignmentID);
                }
            }
            return RedirectToPage("/students/myProgress");
        }
    }
}
