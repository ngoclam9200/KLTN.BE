using DoubleLStore.WebApp.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoubleLStore.WebApp.IService
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
       

        private readonly IDictionary<string, Tuple<string, string>> tokens = new Dictionary<string, Tuple<string, string>>();

        private readonly string key;
        public IDictionary<string, Tuple<string, string>> Tokens => tokens;

        public JwtAuthenticationManager(string key)
        {
            this.key = key;

        }

     
        public string Authenticate(Users user)
        {
             
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {  
                    
                    new Claim("id", user.Id),
                    new Claim("RoleId", user.RoleId),
                    new Claim("username", user.Username),
                    new Claim("Fullname", user.Fullname),
                    new Claim("phonenumber", user.Phonenumber),
                     new Claim("email", user.Email),

                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public JwtSecurityToken GetInFo(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken resulttoken = null;
       
            try
            {
                //resulttoken = handler.ReadJwtToken(abc);
                resulttoken = handler.ReadJwtToken(token.Split(" ", 2)[1].Trim());
            }
            catch (IndexOutOfRangeException e)
            {
                throw new InvalidOperationException("Cannot read token");
            }

            return resulttoken;
        }

        public string TokenResetPassword(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("RoleId", user.RoleId.ToString()),
                    new Claim("username", user.Username),
                    new Claim("Fullname", user.Fullname),
                    new Claim("phonenumber", user.Phonenumber),
                     new Claim("email", user.Email),

                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string Authenticate(Staffs staff)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {

                    new Claim("id", staff.Id),
                    new Claim("RoleId", staff.RoleId),
                    new Claim("username", staff.Username),
                    new Claim("Fullname", staff.Fullname),
                    new Claim("phonenumber", staff.Phonenumber),
                     new Claim("email", staff.Email),

                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
