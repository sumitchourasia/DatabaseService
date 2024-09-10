namespace DBServiceLocal.Dto
{
    public class OrderModels
    {

    }

    public class OrderRequestByOrderId
    { 
        public string OrderId { get; set; }
    }

    public class OrderResponseByOrderId 
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string FeatureId { get; set; }
        public double TotalAmount { get; set; }
        public string OrderPaymentDetails { get; set; }
        public string OrderDetails { get; set; }
        
    }

    public class APIResponse { 
        public int Status { get; set; }
        public string Message { get; set; }
        public List<OrderResponseByOrderId> OrderResponseByOrderId { get; set; }
    }

}
