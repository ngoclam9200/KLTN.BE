namespace DoubleLStore.WebApp.ViewModels.AddressUser
{
    public class CreateAddressUserRequest
    {
        public string UserId { get; set; }
        public string Address { get; set; }

        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public string WardCode { get; set; }
    }
}
