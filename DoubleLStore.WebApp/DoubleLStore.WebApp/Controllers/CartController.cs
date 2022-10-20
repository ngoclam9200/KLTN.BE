using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public CartController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        [HttpGet("get-all-product-in-cart/{userid}")]
        
        public async Task<IActionResult> GetAllProductInCart(string userid)
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


            if ( RoleId == "3")
            {
                var listproduct = await _context.Carts.Where(x => x.UserId == userid).ToListAsync();
                for(int i=0; i<listproduct.Count; i++)
                {
                    var user = await _context.Users.Where(x => x.Id == userid).ToListAsync();
                    var product = await _context.Products.Where(x => x.Id == listproduct[i].ProductId).ToListAsync();
                }    
               
                return Ok(new Response { Status = 200, Message = "Success", Data = listproduct });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("get-count-product-in-cart/{userid}")]

        public async Task<IActionResult> GetCountProductInCart(string userid)
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
            {
                var listproduct = await _context.Carts.Where(x => x.UserId == userid).ToListAsync();
                

                return Ok( listproduct.Count );
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("get-cart-by-id/{id}")]
        
        public async Task<IActionResult> GetCartById(string id)
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
            {
                var cart = await _context.Carts.Where(x => x.Id == id).ToListAsync();
                for (int i = 0; i < cart.Count; i++)
                {
                    var user = await _context.Users.Where(x => x.Id == cart[i].UserId).ToListAsync();
                    var product = await _context.Products.Where(x => x.Id == cart[i].ProductId).ToListAsync();
                }

                return Ok(new Response { Status = 200, Message = "Success", Data = cart });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        
        [HttpPut("increase-product")]
        public async Task<IActionResult> InCreaseProduct([FromBody] IncreaseDecreaseProductRequest request)
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
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!, vui lòng đăng nhập" });


            var findproductincart = await _context.Carts.FindAsync(request.Id);
            if (findproductincart == null)
            {
                return NotFound(new Response { Status = 404, Message = "Sản phẩm không tồn tại trong giỏ hàng" });
            }

            
            try
            {
                var product = await _context.Products.Where(x => x.Id == request.ProductId).ToListAsync();
                if(product[0].Stock>findproductincart.Quantity)
                {
                    findproductincart.Quantity += 1;

                    await _context.SaveChangesAsync();
                }
                else return Ok(new Response { Status = 200, Message = "Số lượng sản phẩm còn lại không đủ", Data = request });



            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Số lượng sản phẩm đã được chỉnh sửa", Data = request });
        }
        
        [HttpPut("decrease-product")]
        public async Task<IActionResult> DeCreaseProduct([FromBody] IncreaseDecreaseProductRequest request)
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
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!, vui lòng đăng nhập" });


            var findproductincart = await _context.Carts.FindAsync(request.Id);
            if (findproductincart == null)
            {
                return NotFound(new Response { Status = 404, Message = "Sản phẩm không tồn tại trong giỏ hàng" });
            }


            try
            {  
                if(findproductincart.Quantity!=1)
                {
                    findproductincart.Quantity -= 1;

                    await _context.SaveChangesAsync();
                }  
                else return Ok(new Response { Status = 200, Message = "Số lượng sản phẩm tối thiểu là 1", Data = request });



            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Số lượng sản phẩm đã được chỉnh sửa", Data = request });
        }



        [HttpPost("create-cart")]

        public async Task<IActionResult> CreateCart(CreateCartRequest request)
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
            {
                var findcart = await _context.Carts.Where(x => x.UserId == request.UserId && x.ProductId == request.ProductId).ToListAsync();
                if (findcart.Count==1)
                {
                    findcart[0].Quantity += 1;
                }
                else
                {
                    Carts cart = new Carts();
                    cart.UserId = request.UserId;
                    cart.ProductId = request.ProductId;
                    cart.Quantity = 1;
                    cart.DateCreated = DateTime.Now;


                    _context.Carts.Add(cart);
                }
                
               
                await _context.SaveChangesAsync();
                return Ok(new Response { Status = 200, Message = "Thêm  sản phẩm vào giỏ hàng thành công" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Thêm   sản phẩm  vào giỏ hàng thất bại" });

        }
        [HttpDelete("delete-cart/{id}")]
        
        public async Task<IActionResult> DeleteCart(string id)
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
            if (RoleId == "3" )
            {

                var cart = await _context.Carts.FindAsync(id);
                if (cart != null)
                {

                    try
                    {
                        //_context.Roles.is(role);
                        _context.Remove(cart);
                        await _context.SaveChangesAsync();
                        return Ok(new Response { Status = 200, Message = "Xóa  sản phẩm thành công!" });
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
        [HttpDelete("delete-all-cart/{userid}")]
         
        public async Task<IActionResult> DeleteAllCaart(string userid)
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
            {

                var cart = await _context.Carts.Where(x=>x.UserId==userid).ToListAsync();
                if (cart != null)
                {

                    try
                    {
                        for(int i=0; i < cart.Count; i++)
                        {
                            _context.Remove(cart[i]);
                        }    
                        //_context.Roles.is(role);
                      
                        await _context.SaveChangesAsync();
                        return Ok(new Response { Status = 200, Message = "Xóa tất cả sản phẩm thành công!" });
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
    }
}
