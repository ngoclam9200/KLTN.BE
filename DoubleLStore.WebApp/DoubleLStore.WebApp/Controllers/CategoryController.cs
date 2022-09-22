using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public CategoryController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }


        [HttpGet("get-all-category")]
        [Authorize]
        public async Task<IActionResult> GetAllCategory()
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


            if (RoleId == "1" || RoleId == "2")
            {
                var listcategory = await _context.Categories.Where(x => x.isDeleted == false).ToListAsync();
                return Ok(new Response { Status = 200, Message = "Success", Data = listcategory });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
         
        [HttpPost("create-category")]

        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
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
            var findname = await _context.Categories.Where(u => u.Name.Trim() == request.Name.Trim()).ToListAsync();
            if (findname.Count != 0)
                return BadRequest(new Response { Status = 400, Message = "Tên loại sản phẩm đã tồn tại" });

            if (RoleId == "1" || RoleId=="2")
            {
                Categories categories = new Categories();
                categories.Name = request.Name;
                categories.DateCreated= DateTime.Now;
                categories.Description = request.Description;
                categories.Image = request.Image;
                //categories.DateCreated = DateTime.Now;
                _context.Categories.Add(categories);
                await _context.SaveChangesAsync();
                return Ok(new Response { Status = 200, Message = "Thêm loại sản phẩm  thành công" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Thêm loại sản phẩm  thất bại" });

        }
        [Authorize]
        [HttpPut("edit-category")]
        public async Task<IActionResult> EditRole([FromBody] EditCategoryRequest request)
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
            if (RoleId != "1" && RoleId !="2")
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!, vui lòng đăng nhập với tài khoản admin hoặc nhân viên" });

             
            var findcategory = await _context.Categories.FindAsync(request.Id);
            if (findcategory == null)
            {
                return NotFound(new Response { Status = 404, Message = "Loại sản phẩm không tồn tại" });
            }

            var checkname = await _context.Categories.Where(s => s.Name == request.Name && s.Id != request.Id).ToListAsync();

            if (checkname.Count != 0)
            {
                return BadRequest(new Response { Status = 400, Message = "Tên loại sản phẩm đã tồn tại, vui lòng thử tên khác" });
            }
            try
            {
                findcategory.Name = request.Name;
                findcategory.Image = request.Image;
                findcategory.Description = request.Description; 
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Loại sản phẩm đã được chỉnh sửa", Data = request });
        }
        [HttpDelete("delete-category/{id}")]
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
            if (RoleId == "1" || RoleId == "2")
            {

                var categories = await _context.Categories.FindAsync(id);
                if (categories != null)
                {

                    try
                    {
                        //_context.Roles.is(role);
                        categories.isDeleted = true;
                        await _context.SaveChangesAsync();
                        return Ok(new Response { Status = 200, Message = "Xóa loại sản phẩm thành công!" });
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
            else return BadRequest(new Response { Status = 400, Message = "Xóa loại sản phẩm thất bại!" });

        }
        [HttpGet("search-category-by-name/{name}")]
        public async Task<IActionResult> SearchRoleByNamr(string name)
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
                var findcategory = await _context.Categories.Where(s => s.Name.StartsWith(name.Trim()) && s.isDeleted == false).ToListAsync();
                if (findcategory.Count > 0)
                    return Ok(new Response { Status = 200, Message = "Success", Data = findcategory });
                else return Ok(new Response { Status = 200, Message = "Không tìm thấy" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Bạn không có quyền tìm kiếm " });


        }



    }
}
