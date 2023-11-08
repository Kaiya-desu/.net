using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using RestApi.Models;

namespace RestApi.Services
{
    public class WarehouseMssqlDbService2 : IWarehouseDbService2
    {
        private readonly string _connString = "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True";
        public async Task<MyStatus>  Transaction(ProductWarehouse productWarehouse)
        {
            int code = 200;
            string msg = "";
            
            try
            {
                var idProduct = productWarehouse.IdProduct;
                var idWarehouse = productWarehouse.IdWarehouse;
                var amount = productWarehouse.Amount;
                var createdAt = productWarehouse.CreatedAt;

                var proc = "AddProductToWarehouse";
                
                using (var connection = new SqlConnection(_connString))
                {
                    await connection.OpenAsync();

                    // Utworzenie transakcji
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (var command = new SqlCommand(proc, connection))
                        {
                            command.Connection = connection;
                            // Przypisujemy, że nasz obiekt SqlCommand wykorzystuje transakcje
                            command.Transaction = transaction;

                            // Wywołanie procedury składowanej
                            command.CommandText = proc;
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("idProduct", idProduct);
                            command.Parameters.AddWithValue("idWarehouse", idWarehouse);
                            command.Parameters.AddWithValue("amount", amount);
                            command.Parameters.AddWithValue("createdAt", createdAt);

                            await command.ExecuteNonQueryAsync();

                            await transaction.CommitAsync();
                            await connection.CloseAsync();
                        }
                    }
                }
                msg = "Procedura wykonana pomyślnie.";
            }
            catch (SqlException ex)
            {
                code = 404;
                msg = ex.Message;
            }
            
            var answer = new MyStatus
            {
                Code = code,
                Message = msg
            };
            
            return answer;
        }
        
        
    }
}
