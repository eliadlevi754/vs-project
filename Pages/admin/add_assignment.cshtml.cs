using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.OleDb;
using System.Diagnostics.Metrics;
using vs_project.App_Code;

namespace vs_project.Pages.admin
{
    public class add_assignmentModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        public add_assignmentModel(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(string Title, string Description, string subject, IFormFile upload)
        {
            string filename = "no file";
            if (upload != null)//if there is a file
            {
                string originalFileName = upload.FileName;
                string extension = Path.GetExtension(originalFileName);
                string onlyFileName = Path.GetFileNameWithoutExtension(originalFileName);
                filename = originalFileName;
                string uploadDir = "";
                
                if (subject.ToLower() == "physics")
                {
                    uploadDir = Path.Combine(_env.WebRootPath, "uploads", "assignments", "physics");
                }
                else if (subject.ToLower() == "math")
                {
                    uploadDir = Path.Combine(_env.WebRootPath, "uploads", "assignments", "math");
                }
                var path = Path.Combine(uploadDir, filename);
                int c = 1;
                while (System.IO.File.Exists(path))
                {
                    filename = $"{onlyFileName}_{c}{extension}";
                    path = Path.Combine(uploadDir, filename);
                    c++;
                }
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await upload.CopyToAsync(stream);
                }
            }
            using (var conn = new OleDbConnection(Imp_Data.ConString))
            {
                conn.Open();

                // 1. Insert the new assignment
                int newAssignmentId = 0;
                string sql = "INSERT INTO Assignments (Title, [Description], [FileName], [subject]) VALUES (?, ?, ?, ?)";
                using (var cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("?", Title);
                    cmd.Parameters.AddWithValue("?", Description ?? "");
                    cmd.Parameters.AddWithValue("?", filename);
                    cmd.Parameters.AddWithValue("?", subject);
                    cmd.ExecuteNonQuery();

                    // 2. Get the ID of the assignment we just created
                    cmd.CommandText = "SELECT @@IDENTITY";
                    newAssignmentId = (int)cmd.ExecuteScalar();
                }

                if (subject.ToLower() == "physics")
                {
                    // 3. Update the counter
                    string sqlUpdate = "UPDATE students SET assignments_due = assignments_due + 1 WHERE physics = True";
                    using (var cmdUpdate = new OleDbCommand(sqlUpdate, conn))
                    {
                        cmdUpdate.ExecuteNonQuery();
                    }

                    // 4. Link students to this specific assignment
                    
                    string addAssignment = @"INSERT INTO [student_assignments] ([studentID], [assignmentID], [isDone])
                            SELECT [Students].[ID], ?, 0 
                            FROM [Students] 
                            WHERE [Students].[physics] = True";

                    using (var cmdAdd = new OleDbCommand(addAssignment, conn))
                    {
                        cmdAdd.Parameters.AddWithValue("?", newAssignmentId);
                        int rowsAffected = cmdAdd.ExecuteNonQuery();

                        
                    }
                }
                if (subject.ToLower() == "math")
                {
                    // 3. Update the counter
                    string sqlUpdate = "UPDATE students SET assignments_due = assignments_due + 1 WHERE math = True";
                    using (var cmdUpdate = new OleDbCommand(sqlUpdate, conn))
                    {
                        cmdUpdate.ExecuteNonQuery();
                    }

                    // 4. Link students to this specific assignment
                    // This is where you were missing the parameter!
                    string addAssignment = @"INSERT INTO [student_assignments] ([studentID], [assignmentID], [isDone])
                            SELECT [Students].[ID], ?, 0 
                            FROM [Students] 
                            WHERE [Students].[math] = True";

                    using (var cmdAdd = new OleDbCommand(addAssignment, conn))
                    {
                        cmdAdd.Parameters.AddWithValue("?", newAssignmentId);
                        int rowsAffected = cmdAdd.ExecuteNonQuery();

                        // Optional: add a breakpoint here to see if rowsAffected is > 0
                    }
                }
            }
            return RedirectToPage("/employees");
        }
    }

    }



