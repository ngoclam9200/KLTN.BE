namespace DoubleLStore.WebApp.ViewModels.Order
{
    public class CreateOrderRequest
    {
      
        public string UserId { get; set; }
        public string AddressShip { get; set; }
        //public double UnitPrice { get; set; }

        public int ShippingFee { get; set; }
      
        public string TransactionId { get; set; }
        public string StatusOrderId { get; set; }
        public  List<OrderDetailRequest> ListProduct { get; set; }
        public int Quantity { get; set; }
        public string Phonenumber { get; set; }
        public string VoucherId { get; set; }
        public string Message { get; set; }
    
    }
}
