namespace DoubleLStore.WebApp.ViewModels.Cart
{
    public class CreateCartRequest
    {
        public string ProductId { get; set; }
       
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public string Size  { get; set; }
    }
}
