using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.OleDb;
using vs_project.App_Code;
using vs_project.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace vs_project.Pages
{
    public class log_inModel : PageModel
    {
        public string msg { get; set; } = "";
        public IActionResult OnPost(string userName, string password)
        {

            string connectionString = Imp_Data.ConString;
            //string connectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = 'C:\Users\eliad\Desktop\computer science project\vs project\AppData\login.accdb'; Persist Security Info = True";
            OleDbConnection con = new(connectionString);

            // בניית פקודת SQL
            string SQLStr = $"SELECT * FROM [users] WHERE [username] = '{userName}' AND password = '{password}';";
            OleDbCommand cmd = new(SQLStr, con);

            // בניית DataSet
            DataSet ds = new DataSet();

            // טעינת הנתונים
            OleDbDataAdapter adapter = new(cmd);
            //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds, "names");

            int count = ds.Tables[0].Rows.Count;
            if (count > 0)

            {

                login user = new login();
                user.Pname = ds.Tables[0].Rows[0]["Pname"].ToString();
                user.Fname = ds.Tables[0].Rows[0]["Fname"].ToString();
                user.username = ds.Tables[0].Rows[0]["username"].ToString();
                user.admin = bool.Parse(ds.Tables[0].Rows[0]["Admin"].ToString());
                string IsAdmin = user.admin == true ? "Admin" : "NotAdmin";

                HttpContext.Session.SetString("Admin", IsAdmin);


                HttpContext.Session.SetString("Username", user.username);
                HttpContext.Session.SetString("FirstName", user.Pname);
                HttpContext.Session.SetString("LastName", user.Fname);

                return RedirectToPage("/Index");
            }
            else
            {
                msg = "Wrong username or password";
                return Page();
            }
        }
    }
}
