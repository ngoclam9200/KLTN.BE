 
using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.Admin;
using DoubleLStore.WebApp.ViewModels.Staff;
using DoubleLStore.WebApp.ViewModels.User;
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
            else
            {
                var findrole = await _context.Roles.Where(u => u.isDeleted == true && u.Id == finduser[0].RoleId).ToListAsync();
                if (findrole.Count != 0) return BadRequest(new Response { Status = 400, Message = "Vai trò nhân viên không tồn tại" });
                else
                {

                    var token = _jwtAuthenticationManager.Authenticate(finduser[0]);

                    if (token == null)
                        return Unauthorized();

                    else

                    {
                        return Ok(new Response { Status = 200, Message = "Đăng nhập thành công", Data = token });
                    }
                }
            }
            
                
            
           

        }
        [HttpGet("get-all-staff")]
        
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

        public async Task<IActionResult> RegisterStaff([FromBody] RegisterStaffRequest request)

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
                staff.Avatar = "https://image.spreadshirtmedia.net/image-server/v1/mp/products/T1459A839PA4459PT28D115912348W9895H10000/views/1,width=550,height=550,appearanceId=839,backgroundColor=F2F2F2/staff-sticker.jpg";
                staff.Gender =request.Gender;
                _context.Staffs.Add(staff);
                //await _context.SaveChangesAsync();

               
                salaryStaff.StaffId = staff.Id;
                salaryStaff.isWorking = true;
                var currentMonth =DateTime.Now.Month.ToString();
                var currentYear =DateTime.Now.Year.ToString();
                salaryStaff.Month = currentMonth+ "/"+currentYear;
                salaryStaff.NumberOfWorking = 0;
                var currentDay=DateTime.Now.Day.ToString();
                salaryStaff.ListDayWorking = "";
                
                salaryStaff.Salary = staff.Salary;
                salaryStaff.SalaryOfThisMonth = "0";
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
  
        public async Task<IActionResult> EditStaff([FromBody] EditStaffRequest request)
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
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePass([FromBody] ChangePassRequest request)
        {

            var finstaff = await _context.Staffs.FindAsync(request.Id);
            if (finstaff == null)
            {
                return NotFound(new Response { Status = 404, Message = "Người dùng không tồn tại" });
            }

            else
            {


                if (finstaff.Password == request.OldPassword)
                {
                    finstaff.Password = request.NewPassword;

                    await _context.SaveChangesAsync();
                    return Ok(new Response { Status = 200, Message = "Mật khẩu đã được chỉnh sửa", Data = request });
                }
                else
                {
                    return BadRequest(new Response { Status = 400, Message = "Mật khẩu cũ không đúng", Data = request });

                }
            }
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
        public async Task<IActionResult> SearchStaffByNameOrEmail(string nameoremail)
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


        [HttpPut("edit-staff-profile")]
        public async Task<IActionResult> EditAdmin([FromBody] EditAdminRequest request)
        {

            var findstaff = await _context.Staffs.FindAsync(request.Id);
            if (findstaff == null)
            {
                return NotFound(new Response { Status = 404, Message = "Người dùng không tồn tại" });
            }




            try
            {

                findstaff.Phonenumber = request.Phonenumber;
                findstaff.Fullname = request.Fullname;
                findstaff.Gender = request.Gender;
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Thông tin nhân viên đã được chỉnh sửa", Data = request });
        }
        [HttpGet("get-staff-by-id/{id}")]

        public async Task<IActionResult> GetStaffById(string id)
        {


            var st = await _context.Staffs.FindAsync(id);
            if (st != null)
            {

                try
                {

                    return Ok(new Response { Status = 200, Message = "Tìm  người dùng thành công!", Data = st });
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
        [HttpPut("edit-avatar-staff")]
        public async Task<IActionResult> EditAvatarAdmin([FromBody] UploadAvatarRequest request)
        {



            var st = await _context.Staffs.FindAsync(request.Id);
            if (st == null)
            {
                return NotFound(new Response { Status = 404, Message = "Người dùng không tồn tại" });
            }




            try
            {
                st.Avatar = request.Avatar;

                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Avatar nhân viên đã được chỉnh sửa", Data = request });
        }



    }
}

