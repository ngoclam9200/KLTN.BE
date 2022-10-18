using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class Orders
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public Users User { get; set; }
        public DateTime OrderDate { get; set; }
        public string AddressShip { get; set; }
     
        
     
        public double  UnitPrice { get; set; }

        public int ShippingFee { get; set; }
        public bool isPaid { get; set; }
        public bool isPaymentOnline { get; set; }
        public string PhoneNumber { get; set; }

        public string TransactionId { get; set; }
        public Transactions Transaction { get; set; }
        
        public string StatusOrderId { get; set; }
        public StatusOrders StatusOrder { get; set; }
        public string VoucherId { get; set; }
        public Vouchers Voucher { get; set; }
        public string Message { get; set; }
        public int Quantity { get; set; }






    }
}
