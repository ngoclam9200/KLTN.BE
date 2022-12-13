namespace DoubleLStore.WebApp.ViewModels.Product
{
    public class CreateProductRequest
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Originalprice { get; set; }
        public int Count { get; set; }
 
        public int Discount { get; set; }
        public string Image { get; set; }
        public bool IsSize { get; set; }
        public int S { get; set; }
        public int M { get; set; }
        public int L { get; set; }
        public int XL { get; set; }
        public int XXL { get; set; }

    }
}
