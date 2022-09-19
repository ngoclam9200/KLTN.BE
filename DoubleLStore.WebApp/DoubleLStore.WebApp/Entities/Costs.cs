namespace DoubleLStore.WebApp.Entities
{
    public class Costs
    {
        public string Id { get; set; }
        public int  Cost { get; set; }
        public string ProductId { get; set; }
        public Products Product { get; set; }
        public string StaffId { get; set; }
        public Staffs Staff { get; set; }
    }
}
