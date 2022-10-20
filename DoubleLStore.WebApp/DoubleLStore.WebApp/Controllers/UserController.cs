using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels;
using DoubleLStore.WebApp.ViewModels.Mail;
using DoubleLStore.WebApp.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace DoubleLStore.WebApp.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public UserController(doubleLStoreDbContext context, IMailService mailService, IConfiguration config, IJwtAuthenticationManager jwtAuthenticationManager)
        {

            _context = context;
            _configuration = config;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _mailService = mailService;

        }
        [HttpPost("login-user")]

        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)

        {
              
            var finduser = await _context.Users.Where(u => (u.Username == request.username || u.Email==request.username) && u.Password == request.password).ToListAsync();
          
          
            if (finduser.Count == 0)
                return BadRequest(new Response { Status = 400, Message = "Sai mật khẩu hoặc tài khoản" });
            else
            {
                if (finduser[0].isVerify == false)
                {
                    return BadRequest(new Response { Status = 400, Message = "Vui lòng xác thực email " });
                }
            }
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
                Users  user = new Users();
                user.Username = request.Username;
                user.DateCreated = DateTime.Now;
                user.RoleId = "3";
                user.Email = request.Email;
                user.Password = request.Password;
                user.Fullname=request.Fullname;
                user.Phonenumber= request.Phonenumber;
                user.Avatar = "https://img.thuthuatphanmem.vn/uploads/2018/09/22/avatar-trang-den-dep_015640236.png";
                user.Gender = "";
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                var finduser = await _context.Users.Where(u => u.Email == request.Email).ToListAsync();
                var token = _jwtAuthenticationManager.TokenResetPassword(finduser[0]);
                var message = new MailRequest();
                message.Subject = "Xác thực email DoubleLStore";
                message.ToEmail = request.Email;
                message.Body = $"Xin chào {request.Fullname}," +
                    "<br/>" +
                    "Xác thực tài khoản tại DoubleLStore" +
                    "<br/>" +
                     "<a style='background-color:black;border:none;color:white;padding:10px 32px;text-align:center;text-decoration:none;display:inline-block;font-size:16px;border-radius:25px;'" + "href =" + "http://localhost:4200/verify-email/" + $"?token={token}" + "> Verify</a>"



                ;
                try
                {
                    await _mailService.SendEmailAsync(message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return Ok(new Response { Status = 200, Message = "Đăng kí thành công" });


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message ="Đăng kí thất bại" });
            }
            

        }
        [HttpGet("get-all-user")]
       
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


            if (RoleId == "1" || RoleId=="2")
            {
                var listuser = await _context.Users.Where(s=>s.isDeleted==false).ToListAsync();
                return Ok(new Response { Status = 200, Message = "Success", Data = listuser });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [Authorize]
        [HttpPut("edit-user")]
        public async Task<IActionResult> EditUser([FromBody] EditUserRequest request)
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
      
        [HttpPut("edit-profile-user")]
        public async Task<IActionResult> EditProfileUser([FromBody] EditProfileRequest request)
        {



            var finduser = await _context.Users.FindAsync(request.Id);
            if (finduser == null)
            {
                return NotFound(new Response { Status = 404, Message = "Người dùng không tồn tại" });
            }

            

          
            try
            {
                finduser.Fullname = request.Fullname;
                finduser.Phonenumber = request.PhoneNumber;
                finduser.Gender = request.Gender;
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Thông tin khách hàng đã được chỉnh sửa", Data = request });
        }
        [HttpPut("edit-avatar-user")]
        public async Task<IActionResult> EditAvatarUser([FromBody] UploadAvatarRequest request)
        {



            var finduser = await _context.Users.FindAsync(request.Id);
            if (finduser == null)
            {
                return NotFound(new Response { Status = 404, Message = "Người dùng không tồn tại" });
            }




            try
            {
                finduser.Avatar = request.Avatar;
                
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Avatar khách hàng đã được chỉnh sửa", Data = request });
        }
        [HttpDelete("delete-user/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string id)
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
        public async Task<IActionResult> SearchUserByNameOrEmail(string nameoremail)
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
        //[HttpPost("sendEmailResetPassword")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GuiEmailKhoiPhucMatKhau([FromBody] RequestSendEmaiResetPassword request)
        //{
        //    if (String.IsNullOrEmpty(request.url))
        //        return BadRequest(new Response { Status = 400, Message = "Thiếu đường dẫn tới trang Khôi phục mật khẩu!" });
           
        //    var finduser = await _context.Users.Where(u => u.Email == request.email).ToListAsync();
        //    if (finduser.Count == 0)
        //        return BadRequest(new Response { Status = 400, Message = "Email chưa đăng ký trong hệ thống!" });
           
            
        //    var token = _jwtAuthenticationManager.TokenResetPassword(finduser[0]);
        //    if (token == null)
        //        return Unauthorized();
        //    var message = new MailRequest();
        //    message.Subject = "Khôi phục mật khẩu";
        //    message.ToEmail = request.email;
        //    message.Body = $"Xin chào {finduser[0].Fullname}." +
        //        "<br/>" +
        //        "Chúng tôi nhận được yêu cầu khôi phục mật khẩu từ bạn. Vui lòng nhấp vào link sau để khôi phục mật khẩu:" + $"{request.url}?token={token}" +
        //        "<br/>" +
        //        "Yêu cầu khôi phục mật khẩu có hiệu lực trong 5 phút" + "<br/>" +
        //             //"<button style='color: #f33f3f;'> Verify</button>"
        //             "<a style='background-color:black;border:none;color:white;padding:10px 32px;text-align:center;text-decoration:none;display:inline-block;font-size:16px;border-radius:25px;'" + "href =" + "http://localhost:4200/verify-email/" + $"?token={token}" + "> Verify</a>"

        //       ;
        //    try
        //    {
        //        await _mailService.SendEmailAsync(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return Ok(new Response { Status = 200, Message = "Gửi yêu cầu thành công, vui lòng kiểm tra email" });
        //}
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword request)
        {
          

            var finduser = await _context.Users.Where(u => u.Email == request.Email).ToListAsync();
            if (finduser.Count == 0)
                return BadRequest(new Response { Status = 400, Message = "Email chưa đăng ký trong hệ thống!" });


            var token = _jwtAuthenticationManager.TokenResetPassword(finduser[0]);
            if (token == null)
                return Unauthorized();
            string randomStr = "";
           

                string[] myIntArray = new string[8];
                int x;
                 
                Random autoRand = new Random();
                for (x = 0; x < 8; x++)
                {
                    myIntArray[x] = Convert.ToChar(Convert.ToInt32(autoRand.Next(65, 87))).ToString();
                    randomStr += (myIntArray[x].ToUpper().ToString());
                }
            
            var message = new MailRequest();
            message.Subject = "Khôi phục mật khẩu";
            message.ToEmail = request.Email;
            message.Body = $"Xin chào {finduser[0].Fullname}." +
                "<br/>" +
                "Mật khẩu mới của bạn là : " + "<b>" + randomStr + "</b>"
                

            ;
            try
            {
                await _mailService.SendEmailAsync(message);
                finduser[0].Password = randomStr;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(new Response { Status = 200, Message = "Gửi yêu cầu thành công, vui lòng kiểm tra email" });
        }
        [HttpPut("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmail request)
        {



            var finduser = await _context.Users.Where(u=>u.Email==request.Email).ToListAsync();
            if (finduser.Count ==0)
            {
                return NotFound(new Response { Status = 404, Message = "Người dùng không tồn tại" });
            }

           
            try
            {
                finduser[0].isVerify = true;
                
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Đã xác thực email" });
        }
        [HttpGet("get-user-by-id/{id}")]

        public async Task<IActionResult> GetUserById(string id)
        {


            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {

                try
                {

                    return Ok(new Response { Status = 200, Message = "Tìm  người dùng thành công!", Data = user });
                }
                catch (IndexOutOfRangeException e)
                {
                    return BadRequest(new Response { Status = 400, Message = e.ToString() });
                }
            }
            else
            {
                return BadRequest(new Response { Status = 400, Message = "Not found" });
            }




        }


    }
}
