
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

            const string sql = "SELECT employess.מזהה, employess.username, employess.address, employess.salary, users.Pname FROM employess INNER JOIN users ON employess.username = users.username;";
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
                    address = reader["address"].ToString(),
                    salary = float.Parse(reader["salary"].ToString()),
                    Pname = reader["Pname"].ToString()
                };
                people.Add(person);
            }
            return people;
        }   
    }
}