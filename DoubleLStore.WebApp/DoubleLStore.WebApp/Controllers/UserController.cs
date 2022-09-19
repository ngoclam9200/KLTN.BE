using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoubleLStore.WebApp.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public UserController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginRequest request)

        {

            //Request.Headers.TryGetValue("Authorization", out var tokenheaderValue);
            var finduser = await _context.Staffs.Where(u => u.Username == request.username && u.Password == request.password).ToListAsync();

            if (finduser.Count == 0)
                return BadRequest(new Response { Status = 400, Message = "Wrong username or password" });

            var token = _jwtAuthenticationManager.Authenticate(finduser[0]);
           
            if (token == null)
                return Unauthorized();
          
            else

            {
                return Ok(new Response { Status = 200, Message = "Login success", Data = token   });
            } 

        }
        [HttpGet("get-all-users")]
 
        public async Task<IActionResult> GetAllUser()
        {
            
                var listrole = await _context.Products.ToListAsync();
                return Ok(new Response { Status = 200, Message = "Success", Data = listrole });
           

        }
    }
}
