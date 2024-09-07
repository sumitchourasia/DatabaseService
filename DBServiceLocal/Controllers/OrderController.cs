using DBServiceLocal.DataAccessLayer;
using DBServiceLocal.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;

namespace DBServiceLocal.Controllers
{
    [Route("api/[controller]")]   //kjviwnvn.com/api/Order/
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IConfiguration _configuration;
        private readonly OrderDAL _orderDAL;
        public OrderController(ILogger<OrderController> logger, 
            IConfiguration configuration, OrderDAL orderDAL) 
        {
            _logger = logger;
            _configuration = configuration;
            _orderDAL = orderDAL;
        }

        [HttpPost("GetOrderDetailByOrderId")]
        public OrderResponseByOrderId GetOrderDetailByOrderId(OrderRequestByOrderId orderRequestByOrderId)
        {
            
            return null;
        }

        [HttpPost]
        [Route("GetOrderDetailByOrderIdAsync")] 
        public async Task<APIResponse> GetOrderDetailByOrderIdAsync(OrderRequestByOrderId orderRequestByOrderId) 
        {
            //_logger.LogInformation($"OrderController : GetOrderDetailByOrderIdAsync : Requesst : {orderRequestByOrderId}");
            _logger.LogInformation($"OrderController : GetOrderDetailByOrderIdAsync : Requesst : {JsonConvert.SerializeObject(orderRequestByOrderId)}");

            if (orderRequestByOrderId == null || string.IsNullOrEmpty(orderRequestByOrderId.OrderId)) {
                return new APIResponse()
                {
                    Status = 301,
                    Message = _configuration.GetValue<string>("ErrorMessage:301")
                };
            }
            string req = JsonConvert.SerializeObject(orderRequestByOrderId);
            OrderRequestByOrderId orderRequestByOrderId1 = JsonConvert.DeserializeObject<OrderRequestByOrderId>(req);

            _logger.LogInformation($"OrderController : GetOrderDetailByOrderIdAsync : OrderID : {orderRequestByOrderId1.OrderId}");

            // call db
            OrderResponseByOrderId dbResponse = await _orderDAL.GetResponseByOrderIdFromDB(orderRequestByOrderId1.OrderId);

            if (dbResponse != null && !string.IsNullOrEmpty(dbResponse.OrderId))
            {
                APIResponse aPIResponse = new APIResponse()
                {
                    Status = 100,
                    Message = "Success",
                    OrderResponseByOrderId = dbResponse
                };
                return aPIResponse;
            }
            else {
                APIResponse aPIResponse = new APIResponse()
                {
                    Status = 301,
                    Message = "Failed",
                    OrderResponseByOrderId = null
                };
                return aPIResponse;
            }

            return new APIResponse();
        }
    }
}
