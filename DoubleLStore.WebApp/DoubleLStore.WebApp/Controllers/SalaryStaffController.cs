using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Collections;
using DoubleLStore.WebApp.ViewModels.SalaryStaff;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryStaffController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public SalaryStaffController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        [HttpGet("get-salary-by-staffid")]
        [Authorize]
        public async Task<IActionResult> GetSalaryById(string staffid)
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

            if(RoleId=="1")
            {
              

                var monthyear = DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                var staff = await _context.Staffs.Where(s => s.Id == staffid).ToListAsync();
                var salary = await _context.SalaryStaffs.Where(x=>x.StaffId == staffid && x.Month==monthyear).ToListAsync();
              
 




                return Ok(new Response { Status = 200, Message = "Success", Data = salary }) ;
            }    
            else
                return BadRequest(new Response { Status = 400, Message = "Bạn không có quyền admin" });




        }
        [Authorize]
        [HttpPut("pay-for-today")]
        public async Task<IActionResult> PayForToday([FromBody] PayForTodayRequest request)
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

             
            var findsalarystaff = await _context.SalaryStaffs.FindAsync(request.Id);
            if (findsalarystaff == null)
            {
                return NotFound(new Response { Status = 404, Message = "Bảng lương không tồn tại không tồn tại" });
            }

            
            try
            {
                findsalarystaff.NumberOfWorking += 1;
                findsalarystaff.ListDayWorking = findsalarystaff.ListDayWorking + ","+request.ListDayWorking;
                int x = Int32.Parse(findsalarystaff.Salary);
                int y = Int32.Parse(findsalarystaff.SalaryOfThisMonth);
                y = y + x / 30;
                findsalarystaff.SalaryOfThisMonth = y.ToString();
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Tính lương hôm nay thành công", Data = findsalarystaff });
        }
        [Authorize]
        [HttpPut("pay-for-month")]
        public async Task<IActionResult> PayForMonth(int id)
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


            var findsalarystaff = await _context.SalaryStaffs.FindAsync(id);
            if (findsalarystaff == null)
            {
                return NotFound(new Response { Status = 404, Message = "Bảng lương không tồn tại không tồn tại" });
            }


            try
            {
                findsalarystaff.isWorking =false;
                
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Tính lương tháng này thành công", Data = findsalarystaff });
        }
    }
}
