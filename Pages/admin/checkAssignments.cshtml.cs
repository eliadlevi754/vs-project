using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.OleDb;
using System.Security.Cryptography.X509Certificates;
using vs_project.App_Code;
using vs_project.Model;

namespace vs_project.Pages.admin
{
    public class checkAssignmentsModel : PageModel
    {
        public List<submission> unGraded = new List<submission>();
        public void OnGet()
        {
            unGraded = new List<submission>();

            string unGradedSql = "SELECT A.[ID], A.[studentID], A.[filePath], A.[assignmentID], B.[Title]" +
                " FROM ([student_assignments] AS A" +
                " INNER JOIN [assignments] AS B ON A.[assignmentID] = B.[ID])" +
                " WHERE A.[isDone] = -1 AND A.[grade] = -1";
            using (var con = new OleDbConnection(Imp_Data.ConString))
            {
                con.Open();
                using (var cmd = new OleDbCommand(unGradedSql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            submission assignment = new submission
                            {
                                submissionID = Convert.ToInt32(reader["ID"]),
                                studentID = Convert.ToInt32(reader["studentID"]),
                                submissionPath = reader["filePath"].ToString(),
                                ID = Convert.ToInt32(reader["assignmentID"]),
                                Title = reader["Title"].ToString()
                            };
                            unGraded.Add(assignment);
                        }
                    }
                }

            }
            

        }
    }
}
