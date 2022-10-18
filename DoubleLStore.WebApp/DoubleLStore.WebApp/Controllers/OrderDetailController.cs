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
    public class OrderDetailController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public OrderDetailController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        [HttpGet("get-orderdetail-by-orderid/{id}")]

        public async Task<IActionResult> GetOrderDetailByOrderId(string id)
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


            if (RoleId == "3" || RoleId == "1" || RoleId=="2")
            {

                var orderdetail = await _context.OrderDetails.Where(x => x.OrderId == id).ToListAsync();
                for (int i = 0; i < orderdetail.Count; i++)
                {
                    var user = await _context.Products.Where(x => x.Id == orderdetail[i].ProductId).ToListAsync();
                    var product = await _context.Orders.Where(x => x.Id == orderdetail[i].OrderId).ToListAsync();
                }

                return Ok(new Response { Status = 200, Message = "Success", Data = orderdetail });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
    }
}
