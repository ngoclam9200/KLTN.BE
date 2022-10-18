namespace DoubleLStore.WebApp.ViewModels.Voucher
{
    public class CreateVoucherRequest
    {
        
        public string Code { get; set; }
        public double Discountprice { get; set; }
        public double Discountfreeship { get; set; }
        
        public DateTime DateExpiration { get; set; }
        public int AmountInput { get; set; }
    

    }
}
