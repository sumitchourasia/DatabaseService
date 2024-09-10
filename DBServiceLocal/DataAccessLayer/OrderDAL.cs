using DBServiceLocal.Controllers;
using DBServiceLocal.Dto;
using MySqlConnector;
using System.Data;

namespace DBServiceLocal.DataAccessLayer
{
    public class OrderDAL
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IConfiguration _configuration;
        public OrderDAL(ILogger<OrderController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<List<OrderResponseByOrderId>> GetResponseByOrderIdFromDB(string OrderId)
        {
            MySqlConnection conn = null;
            MySqlCommand command = null;
            List<OrderResponseByOrderId> orderResponseByOrderId = null;
            try
            {
                conn = new MySqlConnection(_configuration["DataBaseConfig:ConnectionString"]);
                await conn.OpenAsync();
                command = conn.CreateCommand();
                command.CommandText = "select o.OrderId,o.Status,o.UserId,o.ProviderId,o.TotalAmount,od.OrderPaymentDetails,od.OrderDetails from tbl_orders o join tbl_order_details od on o.OrderId = od.OrderId and o.UserId = @transactionid order by 1 desc limit 10";

                command.Parameters.AddWithValue("@transactionid", OrderId);

                var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows == true) {
                    orderResponseByOrderId = new List<OrderResponseByOrderId>();
                    while (reader.Read())
                    {
                        orderResponseByOrderId.Add( new OrderResponseByOrderId
                        {
                            OrderId = reader.GetString("OrderId"),
                            Status = reader.GetString("Status"),
                            UserId = reader.GetString("UserId"),
                            FeatureId = reader.GetString("ProviderId"),
                            TotalAmount =  reader.GetDouble("TotalAmount"),
                            OrderPaymentDetails = reader.GetString("OrderPaymentDetails"),
                            OrderDetails = reader.GetString("OrderDetails")
                        });
                    }
                }

                return orderResponseByOrderId;

            }
            catch (Exception ex)
            {
                orderResponseByOrderId = null;
                _logger.LogError(ex, "OrderDAL : GetResponseByOrderIdFromDB : Exception : ");
            }
            finally 
            {
                if (command != null) 
                {
                    command.Dispose();
                }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return orderResponseByOrderId;
        }
    }
}
