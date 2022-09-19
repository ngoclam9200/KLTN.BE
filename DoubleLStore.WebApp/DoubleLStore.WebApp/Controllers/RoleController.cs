using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels;
using DoubleLStore.WebApp.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {


        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public RoleController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        
        
        [HttpGet("get-all-roles")]
        [Authorize]
        public async Task<IActionResult> GetAllRole()
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
                return BadRequest(new Response { Status = 400, Message =  "Không xác thực được người dùng" });
            }
            RoleId = token.Claims.First(claim => claim.Type == "RoleId").Value;
            

            if (RoleId == "1")
            {
                var listrole = await _context.Roles.ToListAsync();
                return Ok(new Response { Status = 200, Message = "Success", Data = listrole });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("get-role-by-id")]
        [Authorize]
        public async Task<IActionResult> GetRoleById()
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
                return BadRequest(new Response { Status = 400, Message = "Unable to authenticate user" });
            }
            RoleId = token.Claims.First(claim => claim.Type == "RoleId").Value;



            var Role = await _context.Roles.FindAsync(RoleId);
                return Ok(new Response { Status = 200, Message = "Success", Data = Role });
            

        }
        [HttpPost("create-role")]
      
        public async Task<IActionResult> CreateRole(CreateRoleRequest request)
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
            var findvaitro = await _context.Roles.Where(u => u.Name.Trim() == request.name.Trim()).ToListAsync();
            if (findvaitro.Count != 0)
                return BadRequest(new Response { Status = 400, Message = "Tên vai trò đã tồn tại" });

            if (RoleId == "1")
            {
                Roles role = new Roles();
                role.Name = request.name;
                role.DateCreated = DateTime.Now;
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                return Ok(new Response { Status = 200, Message = "Thêm vai trò thành công" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Thêm vai trò thất bại" });

        }
        [Authorize]
        [HttpPut("edit-role-name")]
        public async Task<IActionResult> EditRole([FromBody] RoleModel request)
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
            if (RoleId != "1")
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!, vui lòng đăng nhập với tài khoản admin" });
            
            if (request.Name == null || request.Name.Length == 0)
                return BadRequest(new Response { Status = 400, Message = "tên vai trò bắt buộc" });
            var findrole = await _context.Roles.FindAsync(request.Id);
            if (findrole == null)
            {
                return NotFound(new Response { Status = 404, Message = "Vai trò không tồn tại" });
            }

            var checkname = await _context.Roles.Where(s => s.Name == request.Name && s.Id != request.Id).ToListAsync();

            if (checkname.Count != 0)
            {
                return BadRequest(new Response { Status = 400, Message = "Tên vai trò đã tồn tại, vui lòng thử tên khác" });
            }
            try
            {
                findrole.Name = request.Name;
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Vai trò đã được chỉnh sửa", Data = request });
        }
        [HttpDelete("delete-role/{id}")]
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

                var role = await _context.Roles.FindAsync(id);
                if (role != null)
                {

                    try
                    {
                        _context.Roles.Remove(role);
                        await _context.SaveChangesAsync();
                        return Ok(new Response { Status = 200, Message = "Xóa vai trò thành công!" });
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
            else return BadRequest(new Response { Status = 400, Message = "Xóa vai trò thất bại!" });

        }



    }
}
