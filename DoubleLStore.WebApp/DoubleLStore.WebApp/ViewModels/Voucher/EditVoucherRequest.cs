namespace DoubleLStore.WebApp.ViewModels.Voucher
{
    public class EditVoucherRequest
    {
       public string Id { get; set; }
        public double Discountprice { get; set; }
        public double Discountfreeship { get; set; }
        
        public DateTime DateExpiration { get; set; }
         
    }
}
