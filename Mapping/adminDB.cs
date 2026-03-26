
﻿using System;
using System.Data.OleDb;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using vs_project.App_Code;
using vs_project.Model;

namespace vs_project.mapping

{
    public class AdminDB
    {
        private readonly string connectionString = Imp_Data.ConString;

        public People SelectAllPeople()
        {
            People people = new People();
           // "SELECT employees.Id, employees.username, employees.address, employees.salary, employees.manager, employees.project, employees.isManager " +
           //     "FROM Courses INNER JOIN Person ON Courses.ResponsibleTeacher=Person.Id;";

            const string sql = "SELECT students.מזהה, students.username, students.assignments_submitted, students.assignments_due, students.math, students.physics, students.last_math, students.last_phys, users.Pname FROM students INNER JOIN users ON students.username = users.username;";
            using var connection = new OleDbConnection(connectionString);
            using var command = new OleDbCommand(sql, connection);

            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader!.Read())
            {
                Person person = new Person();
                person = new Person
                {
                    Id = int.Parse(reader["מזהה"].ToString()),
                    username = reader["username"].ToString(),
                    assignments_submitted = int.Parse(reader["assignments_submitted"].ToString()),
                    assignments_due = int.Parse(reader["assignments_due"].ToString()),
                    math = bool.Parse(reader["math"].ToString()),
                    physics = bool.Parse(reader["math"].ToString()),
                    last_math = int.Parse(reader["last_math"].ToString()),
                    last_phys = int.Parse(reader["last_phys"].ToString()),
                    Pname = reader["Pname"].ToString()
                };
                people.Add(person);
            }
            return people;
        }   
    }
}