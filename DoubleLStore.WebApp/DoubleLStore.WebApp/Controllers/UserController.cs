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
                return Ok(new Response { Status = 200, Message = "Đăng kí thành công" });


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message ="Đăng kí thất bại" });
            }
            

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
}
