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
    public class SIgnUpModel : PageModel
    {
        [BindProperty]
        public string username { get; set; }
        [BindProperty]
        public string password { get; set; }
        [BindProperty]
        public string Pname { get; set; }
        [BindProperty]
        public string Fname { get; set; }
        //[BindProperty]
        //public login person { get; set; }
        public string msg { get; set; }
        [BindProperty]
        public bool isAdmin { get; set; }

        [BindProperty]
        public bool vehicle1 { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost(/*string username, string password, string Pname, string Fname*/)
        {
            //connect
            string connectionString = Imp_Data.ConString;
            OleDbConnection con = new(connectionString);

            //check if there is
            string SQLStr = "SELECT * FROM [users] WHERE [username] = ?";
            OleDbCommand cmd = new(SQLStr, con);
            cmd.Parameters.AddWithValue("?", username);

            DataSet ds = new DataSet();

            OleDbDataAdapter adapter = new(cmd);
            adapter.Fill(ds, "names");

            int count = ds.Tables[0].Rows.Count;
            /*if (isAdmin)
            {
                TempData["isAdmin"] = isAdmin;
                TempData["username"] = username;
                TempData["password"] = password;
                TempData["Pname"] = Pname;
                TempData["Fname"] = Fname;
                return Redirect("verifyAdmin");
            }*/
            if (count > 0)
            {
                //msg.Style.Add("color", "red");
                msg = "User Name has been taken, try another one";
                return Page();
            }
            else {
                DataRow dr = ds.Tables["names"].NewRow();
                try
                {
                    dr["username"] = username;
                    dr["password"] = password;
                    dr["Fname"] = Fname;
                    dr["Pname"] = Pname;
                    //bool aaa=  isAdmin;
                    HttpContext.Session.SetString("admin", isAdmin.ToString());
                    dr["admin"] = isAdmin ? -1:0;
                    ds.Tables["names"].Rows.Add(dr);


                    OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                    builder.QuotePrefix = "[";
                    builder.QuoteSuffix = "]";
                    //adapter.UpdateCommand = builder.GetInsertCommand();

                    adapter.Update(ds, "names");

                    return Redirect("/Index");
                }   
                catch
                {
                    return Page();
                }
            }
        }
    }
}
