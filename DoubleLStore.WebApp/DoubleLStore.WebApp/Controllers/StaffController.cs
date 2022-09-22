 
using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public StaffController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        [HttpPost("login-staff")]

        public async Task<IActionResult> LoginStaff([FromBody] LoginStaffRequest request)

        {

            var finduser = await _context.Staffs.Where(u => u.Username == request.username && u.Password == request.password).ToListAsync();

            if (finduser.Count == 0)
                return BadRequest(new Response { Status = 400, Message = "Sai mật khẩu hoặc tài khoản" });

            var token = _jwtAuthenticationManager.Authenticate(finduser[0]);

            if (token == null)
                return Unauthorized();

            else

            {
                return Ok(new Response { Status = 200, Message = "Login success", Data = token });
            }
            return Ok();

        }
        [HttpGet("get-all-staff")]
        [Authorize]
        public async Task<IActionResult> GetAllStaff()
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
                var liststaff = await _context.Staffs.Where(x => x.isDeleted == false).ToListAsync();
                return Ok(new Response { Status = 200, Message = "Success", Data = liststaff });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });
        }
        [HttpPost("register-staff")]

        public async Task<IActionResult> RegisterUser([FromBody] RegisterStaffRequest request)

        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var checkusername = await _context.Staffs.Where(s => s.Username == request.Username).ToListAsync();
            var checkemail = await _context.Staffs.Where(s => s.Email == request.Email).ToListAsync();

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
                SalaryStaff salaryStaff = new SalaryStaff();
                Staffs staff = new Staffs();
                staff.Username = request.Username;
                staff.DateCreated = DateTime.Now;
                staff.RoleId = "2";
                staff.Email = request.Email;
                staff.Password = request.Password;
                staff.Fullname = request.Fullname;
                staff.Phonenumber = request.Phonenumber;
                staff.Salary= request.Salary;
                staff.Avatar = "https://img-cache.coccoc.com/image2?i=3&l=37/815452733";
                staff.Gender =request.Gender;
                _context.Staffs.Add(staff);
                //await _context.SaveChangesAsync();

               
                salaryStaff.StaffId = staff.Id;
                salaryStaff.isWorking = true;
                var currentMonth =DateTime.Now.Month.ToString();
                var currentYear =DateTime.Now.Year.ToString();
                salaryStaff.Month = currentMonth+ "/"+currentYear;
                salaryStaff.NumberOfWorking = 1;
                var currentDay=DateTime.Now.Day.ToString();
                salaryStaff.ListDayWorking = currentDay;
                int x = Int32.Parse(staff.Salary);
                salaryStaff.Salary = staff.Salary;
                salaryStaff.SalaryOfThisMonth = (salaryStaff.NumberOfWorking * (x / 30)).ToString();
                _context.SalaryStaffs.Add(salaryStaff);
                await _context.SaveChangesAsync();


                return Ok(new Response { Status = 200, Message = "Đăng kí thành công" });


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = "Đăng kí thất bại" });
            }


        }
        [Authorize]
        [HttpPut("edit-staff")]
        public async Task<IActionResult> EditRole([FromBody] EditStaffRequest request)
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
            if (RoleId == "3")
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!, vui lòng đăng nhập với tài khoản admin" });


            var findstaff = await _context.Staffs.FindAsync(request.Id);
            if (findstaff == null)
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
                findstaff.Username = request.Username;
                findstaff.Email = request.Email;
                findstaff.Phonenumber = request.Phonenumber;
                findstaff.Fullname = request.Fullname;
                findstaff.Avatar = request.Avatar;
                findstaff.Gender=request.Gender;
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Thông tin khách hàng đã được chỉnh sửa", Data = request });
        }
        [HttpDelete("delete-staff/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteStaff(string id)
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

                var staff = await _context.Staffs.FindAsync(id);
                if (staff != null)
                {

                    try
                    {
                      staff.isDeleted = true;
                        await _context.SaveChangesAsync();
                        return Ok(new Response { Status = 200, Message = "Xóa nhân viên thành công!" });
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        return BadRequest(new Response { Status = 400, Message = e.ToString() });
                    }
                }
                else
                {
                    return BadRequest(new Response { Status = 400, Message = "Không tìm thấy" });
                }


            }
            else return BadRequest(new Response { Status = 400, Message = "Xóa nhân viên  thất bại!" });

        }
        [HttpGet("search-staff-by-nameoremail/{nameoremail}")]
        public async Task<IActionResult> SearchStaffByNamr(string nameoremail)
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
                var findstaff = await _context.Staffs.Where(s => (s.Fullname.StartsWith(nameoremail.Trim()) && s.isDeleted == false) || (s.Email.StartsWith(nameoremail.Trim()) && s.isDeleted == false)).ToListAsync();
                if (findstaff.Count > 0)
                    return Ok(new Response { Status = 200, Message = "Success", Data = findstaff });
                else return Ok(new Response { Status = 200, Message = "Không tìm thấy" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Bạn không có quyền tìm kiếm " });


        }

    }
}

