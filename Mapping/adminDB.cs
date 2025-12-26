
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
            const string sql = "SELECT * FROM [employess];";
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
                    manager = reader["manager"].ToString(),
                    ismanager = bool.Parse(reader["isManager"].ToString()),
                    project = reader["project"].ToString(),
                };
                people.Add(person);
            }
            return people;
        }   
    }
}