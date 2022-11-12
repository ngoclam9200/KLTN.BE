using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase 
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public NotificationController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        [HttpGet("get-all-notification/{userid}")]

        public async Task<IActionResult> GetAllNotification(string userid)
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
                var listnotifi = await _context.Notifis.Where(x => x.UserId == userid).ToListAsync();
                if (listnotifi.Count > 0)
                {
                    for (int i = 0; i < listnotifi.Count; i++)
                    {
                        for (int j = 0; j < listnotifi.Count; j++)
                        {
                            if(listnotifi[j].DateCreated>listnotifi[i].DateCreated)
                            {
                                var tmp =listnotifi[j];
                                listnotifi[j] = listnotifi[i];
                                listnotifi[i] = tmp;
                            }    



                        }
                        ////var user = await _context.Users.Where(x => x.Id == listnotifi[i].UserId).ToListAsync();
                    }
                }

                return Ok(new Response { Status = 200, Message = "Success", Data = listnotifi });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("get-count-notifi-unread/{userid}")]

        public async Task<IActionResult> GetCountNotifi(string userid)
        {



            var count = await _context.Notifis.Where(x => x.UserId == userid && x.isNewNotifi == true).CountAsync();


            return Ok(count);

        }
        [HttpGet("seen-notifi/{userid}")]

        public async Task<IActionResult> SeenNotifi(string userid)
        {



            var listnotifi = await _context.Notifis.Where(x => x.UserId == userid && x.isNewNotifi == true).ToListAsync();

            for(int i= 0;i<listnotifi.Count;i++)
            {
                listnotifi[i].isNewNotifi = false;
            }
            await _context.SaveChangesAsync();
            return Ok(new Response { Status = 200, Message = "Đã xem thông báo"});

        }

    }
}
