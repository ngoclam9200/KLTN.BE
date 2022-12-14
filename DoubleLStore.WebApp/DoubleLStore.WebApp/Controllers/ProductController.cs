using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
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
        
        public async Task<IActionResult> GetAllProduct()
        {
            
                var listproduct = await _context.Products.Where(x => x.isDeleted == false).ToListAsync();
                return Ok(new Response { Status = 200, Message = "Success", Data = listproduct });
             

        }
        [HttpGet("get-lastest-product")]

        public async Task<IActionResult> GetLastestProduct()
        {
            var month= DateTime.Now.Month;
            var year= DateTime.Now.Year;
            ArrayList listdata = new ArrayList();
            var listproduct = await _context.Products.Where(x => x.isDeleted == false).ToListAsync();
            if (listproduct.Count <= 3)
            {
                for (int i = 0; i < listproduct.Count; i++)
                {
                    listdata.Add(listproduct[i]);
                }    
                    return Ok(new Response { Status = 200, Message = "Success", Data = listdata });
            }
            else
            {
                for (int i = 0; i < listproduct.Count; i++)
                {
                    var m = listproduct[i].DateCreated.Month;
                    var y = listproduct[i].DateCreated.Year;
                    if (m == month && y == year) listdata.Add(listproduct[i]);
                    if (listdata.Count == 3) break;
                    if (i == listproduct.Count - 1)
                    {
                        i = -1;
                        if (month == 1)
                        {
                            month = 12;
                            year = year--;

                        }
                        else month--;
                    }
                }
                return Ok(new Response { Status = 200, Message = "Success", Data = listdata });
            }    
           
            
          

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
                products.isSize = request.IsSize;
                _context.Products.Add(products);
                
                 ProductDetail productDetail = new ProductDetail();
                    productDetail.ProductId = products.Id;
                    productDetail.S = request.S;
                    productDetail.M = request.M;
                    productDetail.L = request.L;
                    productDetail.XL = request.XL;
                    productDetail.XXL = request.XXL;
                    _context.ProductDetails.Add(productDetail);
                  
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
       
        [HttpPut("edit-product")]
        public async Task<IActionResult> EditProduct([FromBody] EditProductRequest request)
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
        [HttpDelete("delete-image-product/{id}")]

        public async Task<IActionResult> DeleteImageProduct(string id)
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

                var imageProd = await _context.ImageProducts.FindAsync(id);
                if (imageProd != null)
                {

                    try
                    {
                        //_context.Roles.is(role);
                         _context.Remove(imageProd);
                        await _context.SaveChangesAsync();
                        return Ok(new Response { Status = 200, Message = "Xóa ảnh  sản phẩm thành công!" });
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
        [HttpGet("search-product-in-category/{categoryid}/{name}")]
        public async Task<IActionResult> SearchProductInCategory(string categoryid, string name)
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

            if (RoleId == "1" || RoleId == "2" || RoleId=="3")
            {
                var findprod = await _context.Products.Where(s => s.Name.StartsWith(name.Trim()) && s.isDeleted == false && s.CategoryId==categoryid).ToListAsync();
                if (findprod.Count > 0)
                    return Ok(new Response { Status = 200, Message = "Success", Data = findprod });
                else return Ok(new Response { Status = 200, Message = "Không tìm thấy" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Bạn không có quyền tìm kiếm " });


        }
        [HttpPost("add-image-product")]

        public async Task<IActionResult> AddImageProduct(AddImageProductRequest request)
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
                
                ImageProduct imageproduct = new ImageProduct();
                imageproduct.ProductId = request.ProductId;
                imageproduct.Url = request.Url;
                imageproduct.isDefaut = false;
                _context.ImageProducts.Add(imageproduct);

                await _context.SaveChangesAsync();
                return Ok(new Response { Status = 200, Message = "Thêm ảnh sản phẩm  thành công" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Thêm  sản phẩm  thất bại" });

        }
        [HttpPut("change-image-product")]
         
        public async Task<IActionResult> ChangeImageProduct([FromBody] ChangeImageProductRequest request)
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


            var findimage = await _context.ImageProducts.FindAsync(request.Id);
            if (findimage == null)
            {
                return NotFound(new Response { Status = 404, Message = "Ảnh  không tồn tại" });
            }


            try
            { if(findimage.isDefaut==true)
                {
                    findimage.Url = request.Url;
                    var findproduct = await _context.Products.FindAsync(request.ProductId);
                    findproduct.Image = request.Url;
                }   
            else
                {
                    findimage.Url = request.Url;
 
                }    
             

                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Sản phẩm đã được chỉnh sửa", Data = request });
        }

        [HttpGet("get-all-image-by-id/{id}")]
        public async Task<IActionResult> GetAllImageById(string id)
        {
            
                var findimage = await _context.ImageProducts.Where(s => s.ProductId==id).ToListAsync();
                if (findimage.Count > 0)
                    return Ok(new Response { Status = 200, Message = "Success", Data = findimage });
                else return Ok(new Response { Status = 200, Message = "Không tìm thấy" });
            


        }
        [HttpGet("get-product-by-id/{id}")]
    
        public async Task<IActionResult> GetProductById(string id)
        {
            

                var prod = await _context.Products.FindAsync(id);
                if (prod != null)
                {

                    try
                    {
                        
                        return Ok(new Response { Status = 200, Message = "Lấy sản phẩm thành công!" , Data= prod});
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
        [HttpGet("get-product-detail-by-product-id/{id}")]

        public async Task<IActionResult> GetProductDetailByProductId(string id)
        {


            var prod = await _context.ProductDetails.Where(x=>x.ProductId==id).ToListAsync();
            if (prod != null)
            {

                try
                {

                    return Ok(new Response { Status = 200, Message = "Lấy chi tiết sản phẩm thành công!", Data = prod });
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

        [HttpGet("get-product-by-cateid/{id}")]

        public async Task<IActionResult> GetProductByCateId(string id)
        {
           

                var prod = await _context.Products.Where(p => p.CategoryId == id && p.isDeleted==false).ToListAsync();
                if (prod != null)
                {

                    try
                    {

                        return Ok(new Response { Status = 200, Message = "Lấy sản phẩm thành công!", Data = prod });
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



    }
}
