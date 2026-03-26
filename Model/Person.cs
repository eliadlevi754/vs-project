namespace vs_project.Model
{

        public class Person
        {
            public int Id { get; set; }
            public string username { get; set; }
            public int assignments_submitted { get; set; }
            public int assignments_due { get; set; }
            public bool math { get; set; }
            public bool physics { get; set; }
            public int last_math { get; set; }
            public int last_phys { get; set; }
            public string Pname { get; set; }
    }
        public class People : List<Person>
        {
            public People() { }
            public People(IEnumerable<Person> list)
                    : base(list) { }
        }
    }
