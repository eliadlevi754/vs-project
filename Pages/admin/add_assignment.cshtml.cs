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
                string sqlUpdate = "";
                conn.Open();
                string sql = "INSERT INTO Assignments (Title, Description, FileName, subject) VALUES (@t, @d, @f, @s)";
                using (var cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@t", Title);
                    cmd.Parameters.AddWithValue("@d", Description ?? "");
                    cmd.Parameters.AddWithValue("@f", filename);
                    cmd.Parameters.AddWithValue("@s", subject);
                    cmd.ExecuteNonQuery();
                }
                if (subject.ToLower() == "physics")
                {
                    sqlUpdate = "UPDATE students SET assignments_due = assignments_due + 1 WHERE students.physics = True";
                }
                else if (subject.ToLower() == "math")
                {
                    sqlUpdate = "UPDATE students SET assignments_due = assignments_due + 1 WHERE students.math = True";
                }
                    using (var cmdUpdate = new OleDbCommand(sqlUpdate, conn))
                    {
                        cmdUpdate.ExecuteNonQuery();
                    }
            }
            return RedirectToPage("/employees");
        }
    }

    }



