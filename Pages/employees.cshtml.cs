using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using vs_project.mapping;
using vs_project.Model;

namespace vs_project.Pages
{
    public class employeesModel : PageModel
    {
        public People List { get; set; } = new People();
        public Person person { get; set; } = new Person();
        public void OnGet()
        {
            int done = 0;
            AdminDB dB = new AdminDB();
            List = dB.SelectAllPeople();
        }
    }
}
