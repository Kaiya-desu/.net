using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RestAPI.Models;

namespace RestAPI.Services
{
    public class AnimalMssqlDbService : IAnimalDbService
    {
        private readonly string _connString = "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True";
        public List<Animal> GetAnimalsFromDb(string orderBy)
        {
            if (orderBy is null) orderBy = "";  //bez tego nie działa dla /animals?orderBy=   
            
            var orderList = new List<string> {"name", "description", "category", "area"};
            if (!orderList.Contains(orderBy.ToLower()))
            {
                orderBy = "name";
            }

            var sql = "SELECT * FROM Animal ORDER BY " + orderBy;
            var output = new List<Animal>();
            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;

                    connection.Open();
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        output.Add(new Animal
                            {
                                IdAnimal = int.Parse(dr["IdAnimal"].ToString()),
                                Name = dr["Name"].ToString(),
                                Description = dr["Description"].ToString(),
                                Category = dr["Category"].ToString(),
                                Area = dr["Area"].ToString(),
                            });
                    }
                    connection.Close();
                }
            }
            return output;
        }

        public void AddAnimalToDb(Animal animal)
        {
            
            var sql = "INSERT INTO Animal(Name, Description, Category, Area) VALUES (@name, @description, @category, @area)";
            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("name", animal.Name);
                    command.Parameters.AddWithValue("description", animal.Description);
                    command.Parameters.AddWithValue("category", animal.Category);
                    command.Parameters.AddWithValue("area", animal.Area);

                    connection.Open();
                    command.ExecuteReader();
                }

                connection.Close();
            }

        }

        public void ModifyAnimalInDb(string sIdAnimal, Animal animal)
        {
            
            var sql = "UPDATE Animal SET Name = @name,  Description = @description, Category = @category, Area = @area WHERE IdAnimal = @idAnimal";
            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("name", animal.Name);
                    command.Parameters.AddWithValue("description", animal.Description);
                    command.Parameters.AddWithValue("category", animal.Category);
                    command.Parameters.AddWithValue("area", animal.Area);
                    command.Parameters.AddWithValue("idAnimal", Int32.Parse(sIdAnimal));
                        
                    connection.Open();
                    command.ExecuteReader();
                }
                connection.Close();
            }
        }

        public void DeleteAnimalFromDb(string sIdAnimal)
        {
            var sql = "DELETE FROM Animal WHERE idAnimal = @id";

            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("id", Int32.Parse(sIdAnimal));

                    connection.Open();
                    command.ExecuteReader();
                }
                connection.Close();
            }
        }

        public bool ValidateData(Animal animal)
        {
            string values = animal.ToString();  // Name + "," + Description + "," + Category + "," + Area - pomijam idAnimal - jest identity w bazie wiec ta wartosc mnie nie interesuje
            var animalSplit = values.Split(",");

            for (int i = 0; i < animalSplit.Length; i++) 
            {
                if (string.IsNullOrEmpty(animalSplit[i]) && i != 1) // i != 1 Bo kolumna DESCRIPTION moze byc NULL/puste - ( na podstawie kodu sql)
                {
                    return false;
                }
            }
            return true;
        }
        
        public bool ValidateId(string sIdAnimal)
        {
            var idAnimal = Int32.Parse(sIdAnimal);
            var sql = "SELECT * FROM Animal WHERE idAnimal = @id";

            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("id", idAnimal);

                    connection.Open();
                    SqlDataReader dr = command.ExecuteReader();
                    if (!dr.Read())
                    {
                        return false;
                    }
                }
                connection.Close();
            }
            return true;
        }
        
    }
}