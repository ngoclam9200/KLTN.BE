using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.Admin;
using DoubleLStore.WebApp.ViewModels.User;
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

        public async Task<IActionResult> LoginAdmin([FromBody] LoginAdminRequest request)

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
        
      
        [HttpPut("edit-admin")]
        public async Task<IActionResult> EditAdmin([FromBody] EditAdminRequest request)
        {
            
            var findadmin = await _context.Admins.FindAsync(request.Id);
            if (findadmin == null)
            {
                return NotFound(new Response { Status = 404, Message = "Người dùng không tồn tại" });
            }

           

             
            try
            {
                
                findadmin.Phonenumber = request.Phonenumber;
                findadmin.Fullname = request.Fullname;
                findadmin.Gender = request.Gender;
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Thông tin admin đã được chỉnh sửa", Data = request });
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePass([FromBody] ChangePassRequest request)
        {

            var findadmin = await _context.Admins.FindAsync(request.Id);
            if (findadmin == null)
            {
                return NotFound(new Response { Status = 404, Message = "Người dùng không tồn tại" });
            }

            else
            {  
               
                
                    if (findadmin.Password == request.OldPassword)
                    {
                        findadmin.Password = request.NewPassword;
                        
                        await _context.SaveChangesAsync();
                      return Ok(new Response { Status = 200, Message = "Mật khẩu đã được chỉnh sửa", Data = request });
                     } 
                    else
                {
                    return BadRequest(new Response { Status = 400, Message = "Mật khẩu cũ không đúng", Data = request });

                }






            }    




           
        }
        [HttpGet("get-admin-by-id/{id}")]

        public async Task<IActionResult> GetAdminById(string id)
        {


            var ad = await _context.Admins.FindAsync(id);
            if (ad != null)
            {

                try
                {

                    return Ok(new Response { Status = 200, Message = "Tìm  người dùng thành công!", Data = ad });
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
        [HttpPut("edit-avatar-admin")]
        public async Task<IActionResult> EditAvatarAdmin([FromBody] UploadAvatarRequest request)
        {



            var findad = await _context.Admins.FindAsync(request.Id);
            if (findad == null)
            {
                return NotFound(new Response { Status = 404, Message = "Người dùng không tồn tại" });
            }




            try
            {
                findad.Avatar = request.Avatar;

                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Avatar admin đã được chỉnh sửa", Data = request });
        }






    }
}
