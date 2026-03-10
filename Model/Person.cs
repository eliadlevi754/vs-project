namespace vs_project.Model
{

        public class Person
        {
            public int Id { get; set; }
            public string username { get; set; }
            public string address { get; set; }
            public float salary { get; set; }
            public string manager { get; set; }
            public bool ismanager { get; set; }
            public string project { get; set; }
            public string Pname { get; set; }
    }
        public class People : List<Person>
        {
            public People() { }
            public People(IEnumerable<Person> list)
                    : base(list) { }
        }
    }
