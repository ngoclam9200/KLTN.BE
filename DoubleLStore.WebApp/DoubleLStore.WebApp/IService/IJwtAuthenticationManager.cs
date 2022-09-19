using DoubleLStore.WebApp.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace DoubleLStore.WebApp.IService
{
    public interface IJwtAuthenticationManager
    {
       public string Authenticate(Users user);
        public string Authenticate(Staffs staff);
        public  string TokenResetPassword(Users user);
        public JwtSecurityToken GetInFo(string token);
    }
}
