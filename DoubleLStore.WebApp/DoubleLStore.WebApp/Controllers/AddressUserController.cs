using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.AddressUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressUserController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public AddressUserController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }


        [HttpGet("get-all-address/{userid}")]
 
        public async Task<IActionResult> GetAllAddressByUserId(string userid)
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


            if (RoleId == "1" || RoleId == "2" || RoleId == "3")
            {
                var listaddress = await _context.AddressUsers.Where(x => x.UserId == userid).ToListAsync();
                for(int i=0;i<listaddress.Count; i++)
                {
                    var user =await _context.Users.Where(x=>x.Id==listaddress[i].UserId).ToListAsync();
                }    
                return Ok(new Response { Status = 200, Message = "Success", Data = listaddress });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("get-address-default/{userid}")]
       
        public async Task<IActionResult> GetAddressDefaultByUserId(string userid)
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


            if (RoleId == "1" || RoleId == "2" || RoleId == "3")
            {
                var listaddress = await _context.AddressUsers.Where(x => x.UserId == userid && x.isAddressDefaut==true).ToListAsync();
                for (int i = 0; i < listaddress.Count; i++)
                {
                    var user = await _context.Users.Where(x => x.Id == listaddress[i].UserId).ToListAsync();
                }
                return Ok(new Response { Status = 200, Message = "Địa chỉ mặc định", Data = listaddress });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpPost("create-address")]

        public async Task<IActionResult> CreateAddress(CreateAddressUserRequest request)
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
            var UserId=token.Claims.First(claim=>claim.Type == "id").Value;
            var user = await _context.AddressUsers.Where(x=>x.UserId==UserId).ToListAsync();
            if (RoleId == "3")
            {
                AddressUsers addressuser = new AddressUsers();
                addressuser.UserId = request.UserId;
                addressuser.ProvinceID = request.ProvinceID;
                addressuser.DistrictID = request.DistrictID;
                addressuser.WardCode=request.WardCode;
                addressuser.Address = request.Address;
                addressuser.isDeleted = false;
                if (user.Count>0)  
                {
                    addressuser.isAddressDefaut=false;
                                        
                }
                else addressuser.isAddressDefaut = true;

                _context.AddressUsers.Add(addressuser);


                await _context.SaveChangesAsync();
                return Ok(new Response { Status = 200, Message = "Thêm  địa chỉ thành công" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Thêm địa chỉ  thất bại" });

        }
        
        [HttpPut("edit-address")]
        public async Task<IActionResult> EditAddress([FromBody] EditAddressUserRequest request)
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
            if (RoleId != "3")
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!" });


            var findaddr= await _context.AddressUsers.FindAsync(request.Id);
            if (findaddr == null)
            {
                return NotFound(new Response { Status = 404, Message = "Địa chỉ không tồn tại" });
            }

          
            try
            {
                findaddr.Address = request.Address;
                findaddr.ProvinceID = request.ProvinceID;
                findaddr.DistrictID = request.DistrictID;
                findaddr.WardCode = request.WardCode;
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Địa chỉ đã được chỉnh sửa", Data = request });
        }
        
        [HttpPut("edit-address-default")]
        public async Task<IActionResult> EditAddressDefault([FromBody] EditAddressDefaut request)
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
            if (RoleId != "3")
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!" });


            var findaddr = await _context.AddressUsers.FindAsync(request.Id);
            if (findaddr == null)
            {
                return NotFound(new Response { Status = 404, Message = "Địa chỉ không tồn tại" });
            }


            try
            { var addr=await _context.AddressUsers.ToListAsync();
                for(int i = 0; i < addr.Count; i++)
                {
                    addr[i].isAddressDefaut = false;
                    if (addr[i].Id == request.Id)
                    {
                        addr[i].isAddressDefaut = true;
                        
                    }    
                }    
                
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Địa chỉ đã được chỉnh sửa", Data = request });
        }
    }
}
