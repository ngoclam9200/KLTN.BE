using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public AdminController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        [HttpPost("login-admin")]

        public async Task<IActionResult> LoginStaff([FromBody] LoginAdminRequest request)

        {

            var findadmin = await _context.Admins.Where(u => u.Username == request.username && u.Password == request.password).ToListAsync();

            if (findadmin.Count == 0)
                return BadRequest(new Response { Status = 400, Message = "Sai mật khẩu hoặc tài khoản" });

            var token = _jwtAuthenticationManager.Authenticate(findadmin[0]);

            if (token == null)
                return Unauthorized();

            else

            {
                return Ok(new Response { Status = 200, Message = "Login success", Data = token });
            }
            return Ok();

        }
        [HttpGet("get-all-admin")]
       
        public async Task<IActionResult> GetAllAdmin()
        {
             
            
                var listadmin = await _context.Admins.Where(x => x.isDeleted == false).ToListAsync();
                return Ok(new Response { Status = 200, Message = "Success", Data = listadmin });
           
        }
        
        [Authorize]
        [HttpPut("edit-admin")]
        public async Task<IActionResult> EditAdmin([FromBody] EditAdminRequest request)
        {
            
            var findadmin = await _context.Admins.FindAsync(request.Id);
            if (findadmin == null)
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
                findadmin.Username = request.Username;
                findadmin.Email = request.Email;
                findadmin.Phonenumber = request.Phonenumber;
                findadmin.Fullname = request.Fullname;
                findadmin.Avatar = request.Avatar;
                findadmin.Gender = request.Gender;
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Thông tin admin đã được chỉnh sửa", Data = request });
        }
       
       

    }
}
