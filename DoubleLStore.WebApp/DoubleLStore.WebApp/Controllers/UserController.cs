using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels;
using DoubleLStore.WebApp.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;

namespace DoubleLStore.WebApp.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public UserController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        [HttpPost("login-user")]

        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)

        {
              
            var finduser = await _context.Users.Where(u => u.Username == request.username && u.Password == request.password).ToListAsync();

            if (finduser.Count == 0)
                return BadRequest(new Response { Status = 400, Message = "Wrong username or password" });

            var token = _jwtAuthenticationManager.Authenticate(finduser[0]);
           
            if (token == null)
                return Unauthorized();
          
            else

            {
                return Ok(new Response { Status = 200, Message = "Login success", Data = token   });
            } 

        }
        [HttpPost("register-user")]

        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)

        {
            var checkusername = await _context.Users.Where(s => s.Username == request.Username).ToListAsync();
            var checkemail = await _context.Users.Where(s => s.Email == request.Email).ToListAsync();

            if (checkusername.Count != 0)
            {
                return BadRequest(new Response { Status = 400, Message = "Tên đăng nhập đã tồn tại, vui lòng thử tên khác" });
            }
            if (checkemail.Count != 0)
            {
                return BadRequest(new Response { Status = 400, Message = "Email đã tồn tại, vui lòng thử email khác" });
            }

            try
            {
                //if (!ValidateEmail(request.Email))
                //    return BadRequest(new Response { Status = 422, Message = "Email không hợp lệ" });

                Users user = new Users();
                user.Username = request.Username;
                user.DateCreated = DateTime.Now;
                user.RoleId = "3";
                user.Email = request.Email;
                user.Password = request.Password;
                user.Fullname=request.Fullname;
                user.Phonenumber= request.Phonenumber;
                user.Avatar = "https://img.thuthuatphanmem.vn/uploads/2018/09/22/avatar-trang-den-dep_015640236.png";
                user.Gender = "";
                user.IsVerify = false;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                SendVerifyEmail(request.Email, request.Username, request.Fullname);
                return Ok(new Response { Status = 200, Message = "Đăng kí thành công" });
            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message ="Đăng kí thất bại" });
            }          
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string hashBytes)
        {
            var userName = EncryptedOperation.DecryptedData(hashBytes);
            var user = _context.Users.FirstOrDefault(x => x.Username == userName);
            if (user == null)
                return BadRequest("Invalid request");

            user.IsVerify = true;
            _context.SaveChanges();
            return Ok("Verify successfully");
        }

        private void SendVerifyEmail(string email, string userName, string fullName)
        {
            //1. Api [POST] verify email
            var link2Verify = $"https://{HttpContext.Request.Host.Value}/api/user/verify-email?hashBytes={EncryptedOperation.EncryptedData(userName)}";

            //2. Service send email: google
            var fromAddress = new MailAddress("nguyenvietlongcv@gmail.com", "Double.Store");
            var toAddress = new MailAddress(email, fullName);
            const string fromPassword = "tgmkrbzuldlxstpf";
            const string subject = "Verify email";
            string body = $@"Please click this link below to verify your email
                            {link2Verify}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        private bool ValidateEmail(string email)
        {
            var expression = $@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            return Regex.IsMatch(email, expression, RegexOptions.IgnoreCase);
        }

        [HttpGet("get-all-user")]
        [Authorize]
        public async Task<IActionResult> GetAllCustomer()
        {
            var RoleId = "";
            Request.Headers.TryGetValue("Authorization", out var tokenheaderValue);
            JwtSecurityToken token = null;
            try
            {
                token = _jwtAuthenticationManager.GetInFo(tokenheaderValue);

            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = "Không xác thực được người dùng" });
            }
            RoleId = token.Claims.First(claim => claim.Type == "RoleId").Value;


            if (RoleId == "1")
            {
                var listuser = await _context.Users.Where(s=>s.isDeleted==false).ToListAsync();
                return Ok(new Response { Status = 200, Message = "Success", Data = listuser });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [Authorize]
        [HttpPut("edit-user")]
        public async Task<IActionResult> EditRole([FromBody] EditUserRequest request)
        {
            


            var finduser = await _context.Users.FindAsync(request.Id);
            if (finduser == null)
            {
                return NotFound(new Response { Status = 404, Message = "Người dùng không tồn tại" });
            }

            var checkusername = await _context.Users.Where(s => s.Username == request.Username && s.Id != request.Id).ToListAsync();
            var checkemail = await _context.Users.Where(s => s.Email == request.Email && s.Id != request.Id).ToListAsync();
            
            if (checkusername.Count != 0)
            {
                return BadRequest(new Response { Status = 400, Message = "Tên đăng nhập đã tồn tại, vui lòng thử tên khác" });
            }
            if (checkemail.Count != 0)
            {
                return BadRequest(new Response { Status = 400, Message = "Email đã tồn tại, vui lòng thử email khác" });
            }
            try
            {
                finduser.Username = request.Username;
                finduser.Email = request.Email;
                finduser.Phonenumber = request.Phonenumber;
                finduser.Fullname = request.Fullname;
                finduser.Avatar = request.Avatar;
                finduser.Gender = request.Gender;
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Thông tin khách hàng đã được chỉnh sửa", Data = request });
        }
        [HttpDelete("delete-user/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var RoleId = "";
            Request.Headers.TryGetValue("Authorization", out var tokenheaderValue);
            JwtSecurityToken token = null;
            try
            {
                token = _jwtAuthenticationManager.GetInFo(tokenheaderValue);

            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = "Không xác thực được người dùng" });
            }
            RoleId = token.Claims.First(claim => claim.Type == "RoleId").Value;
            if (RoleId == "1")
            {

                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {

                    try
                    {
                       user.isDeleted = true;
                        await _context.SaveChangesAsync();
                        return Ok(new Response { Status = 200, Message = "Xóa khách hàng thành công!" });
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        return BadRequest(new Response { Status = 400, Message = e.ToString() });
                    }
                }
                else
                {
                    return BadRequest(new Response { Status = 400, Message = "Không tìm thấy khách hàng" });
                }


            }
            else return BadRequest(new Response { Status = 400, Message = "Xóa vai trò thất bại!" });

        }
        [HttpGet("search-customer-by-name/{nameoremail}")]
        public async Task<IActionResult> SearchUserByName(string nameoremail)
        {
            var RoleId = "";
            Request.Headers.TryGetValue("Authorization", out var tokenheaderValue);
            JwtSecurityToken token = null;
            try
            {
                token = _jwtAuthenticationManager.GetInFo(tokenheaderValue);

            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = "Không xác thực được người dùng" });
            }
            RoleId = token.Claims.First(claim => claim.Type == "RoleId").Value;

            if (RoleId == "1" || RoleId=="2")
            {
                var finduser = await _context.Users.Where(s => (s.Fullname.StartsWith(nameoremail.Trim()) && s.isDeleted == false) || (s.Email.StartsWith(nameoremail.Trim()) && s.isDeleted == false)).ToListAsync();
                if (finduser.Count > 0)
                    return Ok(new Response { Status = 200, Message = "Success", Data = finduser });
                else return Ok(new Response { Status = 200, Message = "Không tìm thấy" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Bạn không có quyền tìm kiếm " });


        }
    }

    public class EncryptedOperation
    {
        public static string EncryptedData(string data)
        {
            string hashbytes = "";
            for(int i = 2; i <data.Length; i++)
            {
                hashbytes += data[i];
            }
            for (int i = 0; i < 2; i++)
            {
                hashbytes += data[i];
            }
            return hashbytes;
        }

        public static string DecryptedData(string hashBytes)
        {
            string data = "";
            for(int i = hashBytes.Length - 2; i < hashBytes.Length; i++)
            {
                data += hashBytes[i];
            }

            for (int i = 0; i < hashBytes.Length - 2; i++)
            {
                data += hashBytes[i];
            }



            return data;
        }
    }
}
