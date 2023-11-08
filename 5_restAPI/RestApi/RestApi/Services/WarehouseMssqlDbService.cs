using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using RestApi.Models;

namespace RestApi.Services
{
    public class WarehouseMssqlDbService : IWarehouseDbService
    {
        private readonly string _connString = "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True";
        
        public async Task<MyStatus> ReturnAnswer(ProductWarehouse productWarehouse)
        {
            int code = 200;
            string msg = "";

            // WALIDACJA DANYCH!
            if (!ValidateAmount(productWarehouse))
            {
                code = 404;
                msg = "Zła wartość!, Amount powinno byc większe niż 0";
            }
            else if (!ValidateCreatedAt(productWarehouse)) 
            {
                code = 404;
                msg = "Zła data!";
            }
            else if (! await ValidateId(productWarehouse))
            {
                code = 404;
                msg = "Produkt o danym ID nie istnieje";
            }
            else  if (! await ValidateWarehouse(productWarehouse))
            {
                code = 404;
                msg = "Magazyn o danym ID nie istnieje";
            }
            else if (! await ValidateOrder(productWarehouse)){
                code = 404;
                msg = "Zamówienie o danym ID nie istnieje";
            }
            else if (! await ValidateOrderInProductWarehouse(idO)){
                code = 404;
                msg = "Zamówienie o danym ID zostało już zrealizowane";
            }
            // Koniec walidacji, jeżeli żaden if nie zostanie uruchomiony program przechodzi do else
            else
            {
                await UpdateOrder(idO); 
                msg = "IdProductWarehouse = " + InsertOrder(productWarehouse, idO);
            }

            var answer = new MyStatus
            {
                Code = code,
                Message = msg
            };
            
            return answer;
        }

        public bool ValidateAmount(ProductWarehouse productWarehouse)
        {
            return productWarehouse.Amount > 0;
        }

        public bool ValidateCreatedAt(ProductWarehouse productWarehouse)
        {
            // sql nie przepusci daty spoza tego przedzialu
            DateTime validate1 = new DateTime(1753, 1, 1, 12, 0, 0);
            DateTime validate2 = new DateTime(9999, 12, 31, 11, 59, 59);
            if (productWarehouse.CreatedAt < validate1 || productWarehouse.CreatedAt > validate2)
                return false;
            
            return true;
        }

        public async Task<bool> ValidateId(ProductWarehouse productWarehouse)
        {
            var idProduct = productWarehouse.IdProduct;
            var sql = "SELECT * FROM Product WHERE IdProduct = @idProduct";
           
            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    
                    command.Parameters.AddWithValue("idProduct", idProduct);
                    
                    await connection.OpenAsync();
                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        if (!dr.Read())
                            return false;
                    }
                }
                await connection.CloseAsync();
            }
            return true;
        }
        public async Task<bool> ValidateWarehouse(ProductWarehouse productWarehouse)
        {
            var idWarehouse = productWarehouse.IdWarehouse;
            var sql = "SELECT * FROM Warehouse WHERE IdWarehouse = @idWarehouse";

            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    
                    command.Parameters.AddWithValue("idWarehouse", idWarehouse);
                    
                    await connection.OpenAsync();
                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        if (!dr.Read())
                            return false;
                    }
                }
                await connection.CloseAsync();
            }
            return true;
        }

        private int idO;
        public async Task<bool> ValidateOrder(ProductWarehouse productWarehouse)
        {
            var idProduct = productWarehouse.IdProduct;
            var amount = productWarehouse.Amount;
            var createdAt = productWarehouse.CreatedAt;
            var sql = "SELECT * FROM " + "[Order]" + " WHERE IdProduct = @idProduct AND amount = @amount AND CreatedAt < @CreatedAt";
            idO = 0;
            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    
                    command.Parameters.AddWithValue("idProduct", idProduct);
                    command.Parameters.AddWithValue("amount", amount);
                    command.Parameters.AddWithValue("createdAt", createdAt);
                    
                    await connection.OpenAsync();
                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        while (dr.Read())
                        {
                            idO = int.Parse(dr["IdOrder"].ToString()); // jezeli bylo kilka podobnych zamowien to przypisze najmlodszy
                        }

                        if (idO == 0)
                            return false;
                    }
                }
                await connection.CloseAsync();
            }
            return true;
        }

        public async Task<bool> ValidateOrderInProductWarehouse(int idOrder)
        {
            var sql = "SELECT * FROM Product_Warehouse WHERE IdOrder = @idOrder";
            
            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    
                    command.Parameters.AddWithValue("idOrder", idOrder);
                    
                    await connection.OpenAsync();
                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        if (dr.Read())
                            return false; // jezeli IdOrder znajduje sie juz w DB
                    }
                }
                await connection.CloseAsync();
            }
            return true;
        }

        public async Task UpdateOrder(int idOrder)
        {
            SqlDateTime newTime = DateTime.Now;
            var sql = "UPDATE " + "[Order]" + " SET FulfilledAt = @newTime WHERE IdOrder = @idOrder";

            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    
                    command.Parameters.AddWithValue("newTime", newTime);
                    command.Parameters.AddWithValue("idOrder", idOrder);
                    
                    await connection.OpenAsync();
                    command.ExecuteReader();
                }
                await connection.CloseAsync();
            }
        }

        public async Task<decimal> GetPrice(ProductWarehouse productWarehouse)
        {
            var idProduct = productWarehouse.IdProduct;
                var sql = "SELECT Price FROM Product WHERE IdProduct = @idProduct"; // wyciagam cene produktu

                using (var connection = new SqlConnection(_connString))
                {
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = sql;
                        
                        command.Parameters.AddWithValue("idProduct", idProduct);
                        
                        await connection.OpenAsync();
                        using (SqlDataReader dr = await command.ExecuteReaderAsync())
                        {
                            if (dr.Read())
                                return (decimal) dr["Price"];
                        }
                    }
                    await connection.CloseAsync();
                }
                return 0;
        }

        public async Task<string> InsertOrder(ProductWarehouse productWarehouse, int idOrder)
        {
            var idWarehouse = productWarehouse.IdWarehouse;
            var idProduct = productWarehouse.IdProduct;
            var amount = productWarehouse.Amount;
            var singleUnitPrice = GetPrice(productWarehouse);
            var price = await singleUnitPrice * amount;
            SqlDateTime newTime = DateTime.Now;
            var sql = "INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) OUTPUT Inserted.IdProductWarehouse VALUES (@idWarehouse, @idProduct, @idOrder, @amount, @price, @createdAt) ";

            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    
                    command.Parameters.AddWithValue("idWarehouse", idWarehouse);
                    command.Parameters.AddWithValue("idProduct", idProduct);
                    command.Parameters.AddWithValue("idOrder", idOrder);
                    command.Parameters.AddWithValue("amount", amount);
                    command.Parameters.AddWithValue("price", price);
                    command.Parameters.AddWithValue("createdAt", newTime);
                    
                    await connection.OpenAsync();
                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        if (dr.Read())
                            return dr["IdProductWarehouse"].ToString();
                        command.ExecuteReader();
                    }
                }
                await connection.CloseAsync();
            }
            return "Error?";
        }
        

    }
}