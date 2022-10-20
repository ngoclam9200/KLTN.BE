using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.Voucher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public VoucherController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }


        [HttpGet("get-all-voucher")]
        [Authorize]
        public async Task<IActionResult> GetAllVoucher()
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


            if (RoleId == "1" )
            {
                var voucher = await _context.Vouchers.Where(x => x.isDeleted == false && x.DateExpiration> DateTime.Now).ToListAsync();
                return Ok(new Response { Status = 200, Message = "Success", Data = voucher });
            }
            else return BadRequest(new Response { Status = 400, Message = "Ban khong co quyen truy cap" });

        }
        [HttpPost("create-voucher")]

        public async Task<IActionResult> CreateVoucher(CreateVoucherRequest request)
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
             
            if (RoleId == "1"  )
            {
                Vouchers voucher = new Vouchers();
                
                voucher.DateCreated = DateTime.Now;
                voucher.DateExpiration = request.DateExpiration;
                voucher.Code = request.Code;
                voucher.isDeleted=false;
                voucher.Discountfreeship= request.Discountfreeship;
                voucher.Discountprice= request.Discountprice;
                voucher.AmountInput= request.AmountInput;
                voucher.AmountRemaining= request.AmountInput;
                
                _context.Vouchers.Add(voucher);
                await _context.SaveChangesAsync();
                return Ok(new Response { Status = 200, Message = "Thêm voucher  thành công" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Thêm voucher  thất bại" });

        }
        [Authorize]
        [HttpPut("edit-voucher")]
        public async Task<IActionResult> EditVoucher([FromBody] EditVoucherRequest request)
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
            if (RoleId != "1" )
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!, vui lòng đăng nhập với tài khoản admin hoặc nhân viên" });


            var findvoucher = await _context.Vouchers.FindAsync(request.Id);
            if (findvoucher == null)
            {
                return NotFound(new Response { Status = 404, Message = "Voucher không tồn tại" });
            }

           
            try
            {
                findvoucher.Discountfreeship = request.Discountfreeship;
                findvoucher.Discountprice = request.Discountprice;
                findvoucher.DateExpiration = request.DateExpiration;
            
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Voucher đã được chỉnh sửa", Data = request });
        }
        [HttpDelete("delete-voucher/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVoucher(string id)
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
            if (RoleId == "1" )
            {

                var voucher = await _context.Vouchers.FindAsync(id);
                if (voucher != null)
                {

                    try
                    {
                        //_context.Roles.is(role);
                        voucher.isDeleted = true;
                        await _context.SaveChangesAsync();
                        return Ok(new Response { Status = 200, Message = "Xóa voucher thành công!" });
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
        [HttpGet("search-voucher-by-code/{code}")]
        public async Task<IActionResult> SearchVoucherByCode(string code)
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

            if (RoleId == "1" || RoleId=="3" )
            {
                var findvoucher = await _context.Vouchers.Where(s => s.Code.StartsWith(code.Trim()) && s.isDeleted == false).ToListAsync();
                if (findvoucher.Count > 0 && findvoucher[0].isDeleted==false)
                {
                    if(findvoucher[0].DateExpiration>DateTime.Now && findvoucher[0].AmountRemaining>0)
                    {
                        return Ok(new Response { Status = 200, Message = "Success", Data = findvoucher });
                    }
                    else return BadRequest(new Response { Status = 400, Message = "Không tìm thấy" });
                }

                else return BadRequest(new Response { Status = 200, Message = "Không tìm thấy" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Bạn không có quyền tìm kiếm " });


        }


    }
}
