using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public ProductController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        [HttpGet("get-all-product")]
        [Authorize]
        public async Task<IActionResult> GetAllProduct()
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
                var listproduct = await _context.Products.Where(x => x.isDeleted == false).ToListAsync();
                return Ok(new Response { Status = 200, Message = "Success", Data = listproduct });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }

        [HttpPost("create-product")]

        public async Task<IActionResult> CreateProduct(CreateProductRequest request)
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
                Products products = new Products();
                products.CategoryId = request.CategoryId;
                products.Price = request.Price;
                products.Stock = request.Count;
                products.Count   = request.Count;
                products.Discount = request.Discount;
                products.Originalprice = request.Originalprice;

                products.Name = request.Name;
                products.DateCreated = DateTime.Now;
                products.Description = request.Description;
                products.Image = request.Image;
                products.isDeleted = false;
                _context.Products.Add(products);

                CostProduct costproduct = new CostProduct();
                costproduct.ProductId = products.Id;
                costproduct.Price = products.Price;
                costproduct.Count = products.Count;
                costproduct.TotalCost = products.Count * products.Price;
                var monthyear = DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                costproduct.Month = monthyear;
                _context.CostProducts.Add(costproduct);

                ImageProduct imageproduct = new ImageProduct();
                imageproduct.ProductId = products.Id;
                imageproduct.Url = products.Image;
                imageproduct.isDefaut = true;
                _context.ImageProducts.Add(imageproduct);

                await _context.SaveChangesAsync();
                return Ok(new Response { Status = 200, Message = "Thêm  sản phẩm  thành công" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Thêm  sản phẩm  thất bại" });

        }
        [Authorize]
        [HttpPut("edit-product")]
        public async Task<IActionResult> EditRole([FromBody] EditProductRequest request)
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
            if (RoleId != "1" && RoleId != "2")
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!, vui lòng đăng nhập với tài khoản admin hoặc nhân viên" });


            var findproduct = await _context.Products.FindAsync(request.id);
            if (findproduct == null)
            {
                return NotFound(new Response { Status = 404, Message = "Sản phẩm không tồn tại" });
            }

            
            try
            {
                findproduct.Name = request.Name;
                findproduct.Image = request.Image;
                findproduct.Description = request.Description;
                findproduct.Price= request.Price;
                findproduct.Discount= request.Discount; 
                findproduct.CategoryId = request.CategoryId;
                
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Sản phẩm đã được chỉnh sửa", Data = request });
        }
        [HttpDelete("delete-product/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(string id)
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

                var prod = await _context.Products.FindAsync(id);
                if (prod != null)
                {

                    try
                    {
                        //_context.Roles.is(role);
                        prod.isDeleted = true;
                        await _context.SaveChangesAsync();
                        return Ok(new Response { Status = 200, Message = "Xóa   sản phẩm thành công!" });
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
            else return BadRequest(new Response { Status = 400, Message = "Xóa   sản phẩm thất bại!" });

        }
        [HttpGet("search-product-by-name/{name}")]
        public async Task<IActionResult> SearchProdByName(string name)
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
                var findprod = await _context.Products.Where(s => s.Name.StartsWith(name.Trim()) && s.isDeleted == false).ToListAsync();
                if (findprod.Count > 0)
                    return Ok(new Response { Status = 200, Message = "Success", Data = findprod });
                else return Ok(new Response { Status = 200, Message = "Không tìm thấy" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Bạn không có quyền tìm kiếm " });


        }


    }
}
