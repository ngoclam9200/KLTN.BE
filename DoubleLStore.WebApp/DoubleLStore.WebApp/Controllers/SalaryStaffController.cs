using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Collections;
using DoubleLStore.WebApp.ViewModels.SalaryStaff;
using DoubleLStore.WebApp.Entities;

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
                if(salary.Count==0)
                {    
                    SalaryStaff salaryStaff = new SalaryStaff();
                    salaryStaff.StaffId = staff[0].Id;
                    salaryStaff.isWorking = true;
                    var currentMonth = DateTime.Now.Month.ToString();
                    var currentYear = DateTime.Now.Year.ToString();
                    salaryStaff.Month = currentMonth + "/" + currentYear;
                    salaryStaff.NumberOfWorking = 0;
                    var currentDay = DateTime.Now.Day.ToString();
                    salaryStaff.ListDayWorking = "";

                    salaryStaff.Salary = staff[0].Salary;
                    salaryStaff.SalaryOfThisMonth = "0";
                    _context.SalaryStaffs.Add(salaryStaff);
                    await _context.SaveChangesAsync();
                    var oldmonthyear = (DateTime.Now.Month-1).ToString() + "/" + DateTime.Now.Year.ToString();

                    var oldsalary = await _context.SalaryStaffs.Where(x => x.StaffId == staffid && x.Month == oldmonthyear).ToListAsync();
                    oldsalary[0].isWorking = false;
                    await _context.SaveChangesAsync();
                    var newsalary = await _context.SalaryStaffs.Where(x => x.StaffId == staffid && x.Month == monthyear).ToListAsync();

                    return Ok(new Response { Status = 200, Message = "Success", Data = newsalary });
                }    
 




                else return Ok(new Response { Status = 200, Message = "Success", Data = salary }) ;
            }    
            else
                return BadRequest(new Response { Status = 400, Message = "Bạn không có quyền admin" });




        }
       
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
                if(findsalarystaff.ListDayWorking =="")
                {
                    findsalarystaff.ListDayWorking =request.ListDayWorking;

                }
                else findsalarystaff.ListDayWorking = findsalarystaff.ListDayWorking + "," + request.ListDayWorking;

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
        
        [HttpPut("pay-for-month")]
        public async Task<IActionResult> PayForMonth([FromBody] PayForMonthRequest request)
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
                findsalarystaff.isWorking = false;
                var monthyear = "";
                if (DateTime.Now.Month == 12)
                {
                    monthyear = "1" + "/" + (DateTime.Now.Year + 1).ToString();
                }
                else
                {
                    monthyear = (DateTime.Now.Month + 1).ToString() + "/" + DateTime.Now.Year.ToString();
                }
                SalaryStaff salaryStaffs = new SalaryStaff();
                salaryStaffs.NumberOfWorking = 0;
                salaryStaffs.Salary = findsalarystaff.Salary;
                salaryStaffs.ListDayWorking = "";
                salaryStaffs.Month = monthyear;
                salaryStaffs.SalaryOfThisMonth = "";
                salaryStaffs.isWorking = true;
                salaryStaffs.StaffId = findsalarystaff.StaffId;
                _context.Add(salaryStaffs);
                await _context.SaveChangesAsync();




            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Tính lương tháng này thành công", Data = findsalarystaff });
        }
        //public async Task<IActionResult> PayForMonth([FromBody] PayForMonthRequest request)
        //{
        //    var RoleId = "";
        //    Request.Headers.TryGetValue("Authorization", out var tokenheaderValue);
        //    JwtSecurityToken token = null;
        //    try
        //    {
        //        token = _jwtAuthenticationManager.GetInFo(tokenheaderValue);
        //    }
        //    catch (IndexOutOfRangeException e)
        //    {
        //        return BadRequest(new Response { Status = 400, Message = "Không xác thực được người dùng" });
        //    }
        //    RoleId = token.Claims.First(claim => claim.Type == "RoleId").Value;
        //    if (RoleId != "1")
        //        return BadRequest(new Response { Status = 400, Message = "Không có quyền!, vui lòng đăng nhập với tài khoản admin" });


        //    var findsalarystaff = await _context.SalaryStaffs.FindAsync(request.Id);
        //    if (findsalarystaff == null)
        //    {
        //        return NotFound(new Response { Status = 404, Message = "Bảng lương không tồn tại không tồn tại" });
        //    }


        //    try
        //    {
        //        findsalarystaff.isWorking = false;
        //        var monthyear = (DateTime.Now.Month+1).ToString() + "/" + DateTime.Now.Year.ToString();
        //        SalaryStaff salaryStaffs = new SalaryStaff();
        //        salaryStaffs.NumberOfWorking = 0;
        //        salaryStaffs.Salary = findsalarystaff.Salary;
        //        salaryStaffs.ListDayWorking = "";
        //        salaryStaffs.Month = monthyear;
        //        salaryStaffs.SalaryOfThisMonth = "";
        //        salaryStaffs.isWorking = true;
        //        salaryStaffs.StaffId = findsalarystaff.StaffId;
        //        _context.Add(salaryStaffs);
        //        await _context.SaveChangesAsync();




        //    }
        //    catch (IndexOutOfRangeException e)
        //    {
        //        return BadRequest(new Response { Status = 400, Message = e.ToString() });
        //    }
        //    return Ok(new Response { Status = 200, Message = "Tính lương tháng này thành công", Data = findsalarystaff });
        //}
    }
}
