using DBServiceLocal.Controllers;
using DBServiceLocal.Dto;
using MySqlConnector;

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

        public async Task<OrderResponseByOrderId> GetResponseByOrderIdFromDB(string OrderId)
        {
            OrderResponseByOrderId orderResponseByOrderId = null;
            try
            {
                var conn = new MySqlConnection(_configuration["DataBaseConfig:ConnectionString"]);
                await conn.OpenAsync();
                var command = conn.CreateCommand();
                command.CommandText = "select o.OrderId,o.Status,o.UserId,o.ProviderId,o.TotalAmount,od.OrderPaymentDetails,od.OrderDetails from tbl_orders o join tbl_order_details od on o.OrderId = od.OrderId and o.OrderId = @transactionid";

                command.Parameters.AddWithValue("@transactionid", OrderId);

                var reader = await command.ExecuteReaderAsync();

                if (reader.HasRows == true) {
                    while (reader.Read())
                    {
                        orderResponseByOrderId = new OrderResponseByOrderId
                        {
                            OrderId = reader.GetString("OrderId"),
                            Status = reader.GetString("Status"),
                            UserId = reader.GetString("UserId"),
                            FeatureId = reader.GetString("ProviderId"),
                            TotalAmount =  reader.GetDouble("TotalAmount"),
                            OrderPaymentDetails = reader.GetString("OrderPaymentDetails"),
                            OrderDetails = reader.GetString("OrderDetails")
                        };
                    }
                }

                return orderResponseByOrderId;

            }
            catch (Exception ex)
            {
                orderResponseByOrderId = null;
                return orderResponseByOrderId;
            }
            finally 
            {
            
            }
            return null;
        }
    }
}
