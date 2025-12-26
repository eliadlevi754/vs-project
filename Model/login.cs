namespace vs_project.Model
{
    public class login
    {
            // Corresponds to ds.Tables[0].Rows[0]["Pname"].ToString()
            public string Pname { get; set; }

            // Corresponds to ds.Tables[0].Rows[0]["Fname"].ToString()
            public string Fname { get; set; }

            // Corresponds to ds.Tables[0].Rows[0]["username"].ToString()
            public string username { get; set; }

            // Corresponds to bool.Parse(ds.Tables[0].Rows[0]["Admin"].ToString())
            public bool admin { get; set; }
            public string password { get; set; }
            
        }
    }


