namespace DoubleLStore.WebApp.ViewModels.AddressUser
{
    public class EditAddressUserRequest
    {
        public string Id { get; set; }
       
        public string Address { get; set; }

        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public string WardCode { get; set; }
    }
}
