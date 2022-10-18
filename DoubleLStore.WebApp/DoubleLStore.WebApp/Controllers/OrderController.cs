using BraintreeHttp;
using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.Entities;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.Order;
using DoubleLStore.WebApp.VnPayHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal.Core;
using PayPal.v1.Payments;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly string _clientId;
        private readonly string _secretKey;
        private readonly IConfiguration _configuration;
        public double TyGiaUSD = 23300;
      
        public OrderController(doubleLStoreDbContext context, IConfiguration config, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
            _configuration = config;
        }
        [HttpPost("create-order")]

        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
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


                Orders order = new Orders();
                order.UserId = request.UserId;
                //order.ProductId = request.ProductId;
                order.StatusOrderId = request.StatusOrderId;
                order.ShippingFee = request.ShippingFee;
                order.isPaid = false;
                order.isPaymentOnline = false;
                order.OrderDate = DateTime.Now;
                order.VoucherId = request.VoucherId;
                order.AddressShip = request.AddressShip;
                order.Quantity = request.Quantity;
                order.PhoneNumber = request.Phonenumber;
                order.TransactionId = request.TransactionId;

                //order.UnitPrice = request.UnitPrice;
                order.Message = request.Message;
                double unitPrice = 0;
                for (int i = 0; i < request.ListProduct.Count; i++)
                {
                    var findprod = await _context.Products.FindAsync(request.ListProduct[i].ProductId);
                    unitPrice += findprod.Price - findprod.Price * findprod.Discount / 100;
                   


                }
                
               
                if(request.VoucherId!="0")
                {
                    var voucher = await _context.Vouchers.FindAsync(request.VoucherId);

                    voucher.AmountRemaining -= 1;
                    //if(voucher.Discountfreeship>0)
                    //{
                    //    order.ShippingFee = order.ShippingFee - order.ShippingFee * Convert.ToInt32( voucher.Discountfreeship) / 100;
                    //}    

                }
                order.UnitPrice = unitPrice+order.ShippingFee;

                _context.Orders.Add(order);
                for (int i = 0; i < request.ListProduct.Count; i++)
                {
                     
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.ProductId = request.ListProduct[i].ProductId;
                    orderDetail.OrderId = order.Id;
                    orderDetail.ProductCount = request.ListProduct[i].ProductCount;
                    _context.OrderDetails.Add(orderDetail);

                    var prod = await _context.Products.FindAsync(request.ListProduct[i].ProductId);
                    prod.Stock -= request.ListProduct[i].ProductCount; ;

                    await _context.SaveChangesAsync();


                }
                var cart = await _context.Carts.Where(c => c.UserId == request.UserId).ToListAsync();
                for (int i = 0; i < request.ListProduct.Count; i++)
                {
                    for (int j = 0; j < cart.Count; j++)
                    {
                        if(cart[j].ProductId==request.ListProduct[i].ProductId)
                        {
                             _context.Carts.Remove(cart[j]);
                        }    

                    }

                }


               
                await _context.SaveChangesAsync();
              
          
             

                return Ok(new Response { Status = 200, Message = "Mua sản phẩm thành công" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Mua sản phẩm thất bại " });


        }
        [HttpGet("get-all-waitconfirm-order/{userid}")]
      
        public async Task<IActionResult> GetAllOrderWaitConfirm(string userid)
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
                var listorder = await _context.Orders.Where(x => x.UserId == userid && x.StatusOrderId=="1").ToListAsync();
                if(listorder.Count>0)
                {
                    for(int i =0; i<listorder.Count; i++)
                    {
                        var user = await _context.Users.Where(x => x.Id == listorder[i].UserId).ToListAsync();
                        //var prod = await _context.Products.Where(x => x.Id == listorder[i].ProductId).ToListAsync();
                        var transactions = await _context.Transactions.Where(x => x.Id == listorder[i].TransactionId).ToListAsync();
                        //var shippingFee = await _context.ShippingFees.Where(x => x.Id == listorder[i].ShippingFeeId).ToListAsync();
                        if(listorder[i].VoucherId!="0")
                        {
                            var voucher = await _context.Vouchers.Where(x => x.Id == listorder[i].VoucherId).ToListAsync();
                        }    

                    }
                }    
               
                return Ok(new Response { Status = 200, Message = "Success", Data = listorder });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("get-all-delivering-order/{userid}")]

        public async Task<IActionResult> GetAllOrderDelivering(string userid)
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
                var listorder = await _context.Orders.Where(x => x.UserId == userid && x.StatusOrderId == "2").ToListAsync();
                if (listorder.Count > 0)
                {
                    for (int i = 0; i < listorder.Count; i++)
                    {
                        var user = await _context.Users.Where(x => x.Id == listorder[i].UserId).ToListAsync();
                        //var prod = await _context.Products.Where(x => x.Id == listorder[i].ProductId).ToListAsync();
                        var transactions = await _context.Transactions.Where(x => x.Id == listorder[i].TransactionId).ToListAsync();
                        //var shippingFee = await _context.ShippingFees.Where(x => x.Id == listorder[i].ShippingFeeId).ToListAsync();
                        if (listorder[i].VoucherId != "0")
                        {
                            var voucher = await _context.Vouchers.Where(x => x.Id == listorder[i].VoucherId).ToListAsync();
                        }


                    }
                }

                return Ok(new Response { Status = 200, Message = "Success", Data = listorder });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("get-all-delivered-order/{userid}")]

        public async Task<IActionResult> GetAllOrderDelivered(string userid)
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
                var listorder = await _context.Orders.Where(x => x.UserId == userid && x.StatusOrderId == "3").ToListAsync();
                if (listorder.Count > 0)
                {
                    for (int i = 0; i < listorder.Count; i++)
                    {
                        var user = await _context.Users.Where(x => x.Id == listorder[i].UserId).ToListAsync();
                        //var prod = await _context.Products.Where(x => x.Id == listorder[i].ProductId).ToListAsync();
                        var transactions = await _context.Transactions.Where(x => x.Id == listorder[i].TransactionId).ToListAsync();
                        //var shippingFee = await _context.ShippingFees.Where(x => x.Id == listorder[i].ShippingFeeId).ToListAsync();
                        if (listorder[i].VoucherId != "0")
                        {
                            var voucher = await _context.Vouchers.Where(x => x.Id == listorder[i].VoucherId).ToListAsync();
                        }


                    }
                }

                return Ok(new Response { Status = 200, Message = "Success", Data = listorder });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("get-all-cancle-order/{userid}")]

        public async Task<IActionResult> GetAllOrderCancle(string userid)
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
                var listorder = await _context.Orders.Where(x => x.UserId == userid && x.StatusOrderId == "4").ToListAsync();
                if (listorder.Count > 0)
                {
                    for (int i = 0; i < listorder.Count; i++)
                    {
                        var user = await _context.Users.Where(x => x.Id == listorder[i].UserId).ToListAsync();
                        //var prod = await _context.Products.Where(x => x.Id == listorder[i].ProductId).ToListAsync();
                        var transactions = await _context.Transactions.Where(x => x.Id == listorder[i].TransactionId).ToListAsync();
                        //var shippingFee = await _context.ShippingFees.Where(x => x.Id == listorder[i].ShippingFeeId).ToListAsync();
                        if (listorder[i].VoucherId != "0")
                        {
                            var voucher = await _context.Vouchers.Where(x => x.Id == listorder[i].VoucherId).ToListAsync();
                        }


                    }
                }

                return Ok(new Response { Status = 200, Message = "Success", Data = listorder });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("admin-get-all-waitconfirm-order")]

        public async Task<IActionResult> AdminGetAllOrderWaitConfirm()
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
                var listorder = await _context.Orders.Where(x => x.StatusOrderId == "1").ToListAsync();
                if (listorder.Count > 0)
                {
                    for (int i = 0; i < listorder.Count; i++)
                    {
                        var user = await _context.Users.Where(x => x.Id == listorder[i].UserId).ToListAsync();
                        //var prod = await _context.Products.Where(x => x.Id == listorder[i].ProductId).ToListAsync();
                        var transactions = await _context.Transactions.Where(x => x.Id == listorder[i].TransactionId).ToListAsync();
                        //var shippingFee = await _context.ShippingFees.Where(x => x.Id == listorder[i].ShippingFeeId).ToListAsync();
                        if (listorder[i].VoucherId != "0")
                        {
                            var voucher = await _context.Vouchers.Where(x => x.Id == listorder[i].VoucherId).ToListAsync();
                        }

                    }
                }

                return Ok(new Response { Status = 200, Message = "Success", Data = listorder });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("admin-get-count-waitconfirm-order")]

        public async Task<IActionResult> AdminGetCountOrderWaitConfirm()
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
                var listorder = await _context.Orders.Where(x => x.StatusOrderId == "1").ToListAsync();
             

                return Ok(new Response { Status = 200, Message = "Success", Data = listorder.Count });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("admin-get-all-delivering-order")]

        public async Task<IActionResult> AdminGetAllOrderDelivering()
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
                var listorder = await _context.Orders.Where(x => x.StatusOrderId == "2").ToListAsync();
                if (listorder.Count > 0)
                {
                    for (int i = 0; i < listorder.Count; i++)
                    {
                        var user = await _context.Users.Where(x => x.Id == listorder[i].UserId).ToListAsync();
                        //var prod = await _context.Products.Where(x => x.Id == listorder[i].ProductId).ToListAsync();
                        var transactions = await _context.Transactions.Where(x => x.Id == listorder[i].TransactionId).ToListAsync();
                        //var shippingFee = await _context.ShippingFees.Where(x => x.Id == listorder[i].ShippingFeeId).ToListAsync();
                        if (listorder[i].VoucherId != "0")
                        {
                            var voucher = await _context.Vouchers.Where(x => x.Id == listorder[i].VoucherId).ToListAsync();
                        }

                    }
                }

                return Ok(new Response { Status = 200, Message = "Success", Data = listorder });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("admin-get-all-delivered-order")]

        public async Task<IActionResult> AdminGetAllOrderDelivered()
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
                var listorder = await _context.Orders.Where(x =>   x.StatusOrderId == "3").ToListAsync();
                if (listorder.Count > 0)
                {
                    for (int i = 0; i < listorder.Count; i++)
                    {
                        var user = await _context.Users.Where(x => x.Id == listorder[i].UserId).ToListAsync();
                        //var prod = await _context.Products.Where(x => x.Id == listorder[i].ProductId).ToListAsync();
                        var transactions = await _context.Transactions.Where(x => x.Id == listorder[i].TransactionId).ToListAsync();
                        //var shippingFee = await _context.ShippingFees.Where(x => x.Id == listorder[i].ShippingFeeId).ToListAsync();
                        if (listorder[i].VoucherId != "0")
                        {
                            var voucher = await _context.Vouchers.Where(x => x.Id == listorder[i].VoucherId).ToListAsync();
                        }

                    }
                }

                return Ok(new Response { Status = 200, Message = "Success", Data = listorder });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
        [HttpGet("admin-get-all-cancle-order")]

        public async Task<IActionResult> AdminGetAllOrderCancle()
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
                var listorder = await _context.Orders.Where(x =>  x.StatusOrderId == "4").ToListAsync();
                if (listorder.Count > 0)
                {
                    for (int i = 0; i < listorder.Count; i++)
                    {
                        var user = await _context.Users.Where(x => x.Id == listorder[i].UserId).ToListAsync();
                        //var prod = await _context.Products.Where(x => x.Id == listorder[i].ProductId).ToListAsync();
                        var transactions = await _context.Transactions.Where(x => x.Id == listorder[i].TransactionId).ToListAsync();
                        //var shippingFee = await _context.ShippingFees.Where(x => x.Id == listorder[i].ShippingFeeId).ToListAsync();
                        if (listorder[i].VoucherId != "0")
                        {
                            var voucher = await _context.Vouchers.Where(x => x.Id == listorder[i].VoucherId).ToListAsync();
                        }

                    }
                }

                return Ok(new Response { Status = 200, Message = "Success", Data = listorder });
            }
            else return BadRequest(new Response { Status = 400, Message = "Not found" });

        }
       
        [HttpPut("confirm-order")]
        public async Task<IActionResult> ConfirmOrder(ConfirmOrderRequest request)
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
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!" });


            var findorder = await _context.Orders.FindAsync(request.Id);
            if (findorder == null)
            {
                return NotFound(new Response { Status = 404, Message = "Đơn hàng không tồn tại" });
            }


            try
            {
                findorder.StatusOrderId = "2";
                Notifi notifi= new Notifi();
                notifi.DateCreated=DateTime.Now;
                notifi.UserId=findorder.UserId;
                notifi.Message = "Đơn hàng" + " " + findorder.Id + " " + "đang giao đến cho bạn";
                _context.Notifis.Add(notifi);

                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Đơn hàng đã được xác nhận ", Data = findorder });
        }
        [HttpPut("cancle-order")]
        public async Task<IActionResult> CancleOrder(ConfirmOrderRequest request)
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
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!" });


            var findorder = await _context.Orders.FindAsync(request.Id);
            if (findorder == null)
            {
                return NotFound(new Response { Status = 404, Message = "Đơn hàng không tồn tại" });
            }


            try
            {
             
                findorder.StatusOrderId = "4";
                //var prod = await _context.Products.FindAsync(findorder.ProductId);
                //prod.Stock += 1;
                Notifi notifi = new Notifi();
                notifi.DateCreated = DateTime.Now;
                notifi.UserId = findorder.UserId;
                notifi.Message = "Đơn hàng" + " " + findorder.Id + " " + "đã hủy";
                _context.Notifis.Add(notifi);
                if (findorder.VoucherId != "0")
                {
                    var voucher = await _context.Vouchers.FindAsync(findorder.VoucherId);

                    voucher.AmountRemaining += 1;
                }
                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Đơn hàng đã hủy", Data = findorder });
        }
        [HttpPut("delivered-order")]
        public async Task<IActionResult> DeliveredOrder(ConfirmOrderRequest request)
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
                return BadRequest(new Response { Status = 400, Message = "Không có quyền!" });


            var findorder = await _context.Orders.FindAsync(request.Id);
            if (findorder == null)
            {
                return NotFound(new Response { Status = 404, Message = "Đơn hàng không tồn tại" });
            }


            try
            {
                findorder.StatusOrderId = "3";
                if(findorder.VoucherId!="0")
                {
                    CostVoucher costVoucher = new CostVoucher();
                    costVoucher.DateCreated = DateTime.Now;
                    costVoucher.VoucherId = findorder.VoucherId;
                    var findvoucher= await _context.Vouchers.FindAsync(findorder.VoucherId);
                    var listOrderProd = await _context.OrderDetails.Where(x=>x.OrderId==findorder.Id).ToListAsync();
                    //var findshippingFee = await _context.ShippingFees.FindAsync(findorder.ShippingFeeId);
                    if (findvoucher.Discountfreeship>0)
                    {
                        costVoucher.Cost = findorder.ShippingFee;

                    }   
                    else
                    {
                        var total = 0;
                        for (int i =0; i <listOrderProd.Count; i++)
                        {
                            var prod = await _context.Products.FindAsync(listOrderProd[i].ProductId);
                            total += listOrderProd[i].ProductCount * (Convert.ToInt32(prod.Price - prod.Price * prod.Discount / 100));
                           
                        }
                        var costvoucher = total * findvoucher.Discountprice / 100;
                        costVoucher.Cost = Convert.ToInt32(costvoucher);
                       
                    }
                    
                    _context.CostVouchers.Add(costVoucher);

                }    
               

                await _context.SaveChangesAsync();


            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Response { Status = 400, Message = e.ToString() });
            }
            return Ok(new Response { Status = 200, Message = "Đơn hàng đã giao", Data = findorder });
        }
        [HttpPost("PaymentPaypal")]
        public async Task<IActionResult> PaymentPaypal(PaymentPayPalRequest paymentPayPalRequest)
        {

       
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);


            #region Create Paypal Order

            double total ;
            
            total = Convert.ToDouble( paymentPayPalRequest.Total/23884);
            total=Math.Round(total,2);
          
            #endregion
            var paypalOrderId = DateTime.Now.Ticks;
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = total.ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = total.ToString()
                            }
                        },
                        //ItemList = itemList,
                        Description = $"Invoice #{paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = $"{hostname}",
                    ReturnUrl = paymentPayPalRequest.returnUrl,
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);
            string paypalRedirectUrl = null;
            try
            {

                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();

                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }

                // return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return BadRequest(new Response { Status = 400, Message = httpException.Message.ToString() });

            }
            return Ok(new Response { Status = 200, Message = "success", Data = paypalRedirectUrl });
        }
        [HttpPost("CheckoutPaypal")]
        public async Task<IActionResult> CheckoutPaypal(RequestCheckoutPaypalModel request)
        {
            

            using var httpClient = new System.Net.Http.HttpClient();
            httpClient.BaseAddress = new Uri("http://example.com/");
            httpClient.DefaultRequestHeaders
      .Accept
      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            var authenticationString = $"Ae_JJ2WlQiJHYJwjPM0aLXfsp6b1_-_u-2m3ae3osShJoyoD6Y2MJm9A0yx-yPcZa9Kcpe6ITEADpacy:ECHo3WAUd7uZ7qvI7IW7edfKZjk9B_KVSAFUWwzoj3_idyz9uhNLTjXth3z0mnCPw-k1smxAUqZlCqYJ";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            var httpMessageRequest = new HttpRequestMessage();
            httpMessageRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            httpMessageRequest.Method = HttpMethod.Post;
            httpMessageRequest.RequestUri = new Uri("https://api.sandbox.paypal.com/v1/payments/payment/" + request.paymentId + "/execute");
 

            string data = "{\r\n                \"" + "payer_id" + "\" : \"" + request.PayerID + "\"   }";
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            
            httpMessageRequest.Content = content;
            var httpResponseMessage = await httpClient.SendAsync(httpMessageRequest);

            if (httpResponseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                return BadRequest(new Response { Status = 400, Message = "Thanh toán lỗi, vui lòng thử lại!" });

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


                Orders order = new Orders();
                order.UserId = request.UserId;
                //order.ProductId = request.ProductId;
                order.StatusOrderId = request.StatusOrderId;
                order.ShippingFee = request.ShippingFee;
                order.isPaid = true;
                order.isPaymentOnline = true;
                order.OrderDate = DateTime.Now;
                order.VoucherId = request.VoucherId;
                order.AddressShip = request.AddressShip;
                order.Quantity = request.Quantity;
                order.PhoneNumber = request.Phonenumber;
                order.TransactionId = request.TransactionId;

                //order.UnitPrice = request.UnitPrice;
                order.Message = request.Message;
                double unitPrice = 0;
                for (int i = 0; i < request.ListProduct.Count; i++)
                {
                    var findprod = await _context.Products.FindAsync(request.ListProduct[i].ProductId);
                    unitPrice += findprod.Price - findprod.Price * findprod.Discount / 100;



                }


                if (request.VoucherId != "0")
                {
                    var voucher = await _context.Vouchers.FindAsync(request.VoucherId);

                    voucher.AmountRemaining -= 1;
                    //if(voucher.Discountfreeship>0)
                    //{
                    //    order.ShippingFee = order.ShippingFee - order.ShippingFee * Convert.ToInt32( voucher.Discountfreeship) / 100;
                    //}    

                }
                order.UnitPrice = unitPrice + order.ShippingFee;

                _context.Orders.Add(order);
                for (int i = 0; i < request.ListProduct.Count; i++)
                {

                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.ProductId = request.ListProduct[i].ProductId;
                    orderDetail.OrderId = order.Id;
                    orderDetail.ProductCount = request.ListProduct[i].ProductCount;
                    _context.OrderDetails.Add(orderDetail);

                    var prod = await _context.Products.FindAsync(request.ListProduct[i].ProductId);
                    prod.Stock -= request.ListProduct[i].ProductCount; ;

                    await _context.SaveChangesAsync();


                }
                var cart = await _context.Carts.Where(c => c.UserId == request.UserId).ToListAsync();
                for (int i = 0; i < request.ListProduct.Count; i++)
                {
                    for (int j = 0; j < cart.Count; j++)
                    {
                        if (cart[j].ProductId == request.ListProduct[i].ProductId)
                        {
                            _context.Carts.Remove(cart[j]);
                        }

                    }

                }



                await _context.SaveChangesAsync();




                return Ok(new Response { Status = 200, Message = "Tạo hóa đơn thành công" });
            }
            else return BadRequest(new Response { Status = 400, Message = "Tạo hóa đơn thất bại " });
        }
        [HttpPost("paymentVnPay")]
        public async Task<IActionResult> paymentVnPay([FromBody] ThanhToanVnPayModel request)
        {

            //Get Config Info
            //string vnp_Returnurl = _configuration["VnPaySettings:vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = _configuration["VnPaySettings:vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = _configuration["VnPaySettings:vnp_TmnCode"]; //Ma website
            string vnp_HashSecret = _configuration["VnPaySettings:vnp_HashSecret"]; //Chuoi bi mat
            if (string.IsNullOrEmpty(request.vnp_Returnurl))
                return BadRequest(new Response { Status = 400, Message = "Thiếu vnp_Returnurl!" });
            if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            {
                return BadRequest((new Response { Status = 400, Message = "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config" }));
            }
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


                Orders order = new Orders();
                order.UserId = request.UserId;
                order.StatusOrderId = request.StatusOrderId;
                order.ShippingFee = request.ShippingFee;
                order.isPaid = false;
                order.isPaymentOnline = false;
                order.OrderDate = DateTime.Now;
                order.VoucherId = request.VoucherId;
                order.AddressShip = request.AddressShip;
                order.Quantity = request.Quantity;
                order.PhoneNumber = request.Phonenumber;
                order.TransactionId = request.TransactionId;

                 
                order.Message = request.Message;
                double unitPrice = 0;
                for (int i = 0; i < request.ListProduct.Count; i++)
                {
                    var findprod = await _context.Products.FindAsync(request.ListProduct[i].ProductId);
                    unitPrice += findprod.Price - findprod.Price * findprod.Discount / 100;



                }


                if (request.VoucherId != "0")
                {
                    var voucher = await _context.Vouchers.FindAsync(request.VoucherId);

                    voucher.AmountRemaining -= 1;
                     

                }
                order.UnitPrice = unitPrice + order.ShippingFee;

                _context.Orders.Add(order);
                for (int i = 0; i < request.ListProduct.Count; i++)
                {

                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.ProductId = request.ListProduct[i].ProductId;
                    orderDetail.OrderId = order.Id;
                    orderDetail.ProductCount = request.ListProduct[i].ProductCount;
                    _context.OrderDetails.Add(orderDetail);

                    var prod = await _context.Products.FindAsync(request.ListProduct[i].ProductId);
                    prod.Stock -= request.ListProduct[i].ProductCount; ;

                    //await _context.SaveChangesAsync();


                }
                var cart = await _context.Carts.Where(c => c.UserId == request.UserId).ToListAsync();
                for (int i = 0; i < request.ListProduct.Count; i++)
                {
                    for (int j = 0; j < cart.Count; j++)
                    {
                        if (cart[j].ProductId == request.ListProduct[i].ProductId)
                        {
                            _context.Carts.Remove(cart[j]);
                        }

                    }

                }
                await _context.SaveChangesAsync();
                // tạo request VNPAY
                VnPayLibrary pay = new VnPayLibrary();
                pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
                pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
                pay.AddRequestData("vnp_TmnCode", vnp_TmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
                pay.AddRequestData("vnp_Amount", (order.UnitPrice * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
                if (!String.IsNullOrEmpty(request.bankCode))
                {
                    pay.AddRequestData("vnp_BankCode", request.bankCode);
                }
                //pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
                pay.AddRequestData("vnp_CreateDate", order.OrderDate.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
                //pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss

                pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
                pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(HttpContext)); //Địa chỉ IP của khách hàng thực hiện giao dịch
                if (!string.IsNullOrEmpty(request.vnpLocale))
                {
                    pay.AddRequestData("vnp_Locale", request.vnpLocale);
                }
                else
                {
                    pay.AddRequestData("vnp_Locale", "vn");
                }
                //pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
                pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.Id); //Thông tin mô tả nội dung thanh toán
                 //pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + "23213123"); //Thông tin mô tả nội dung thanh toán

                pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
                pay.AddRequestData("vnp_ReturnUrl", request.vnp_Returnurl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
                                                                            //pay.AddRequestData("vnp_TxnRef", order.Id); //mã hóa đơn
                pay.AddRequestData("vnp_TxnRef", order.Id); //mã hóa đơn


                pay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddDays(3).ToString("yyyyMMddHHmmss"));
                string paymentUrl = pay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                return Ok((new Response { Status = 200, Message = "Ok", Data = paymentUrl }));
 
            }
            else return BadRequest((new Response { Status = 400, Message = "Thanh toán thất bại" }));












        }
        [HttpPost("checkout-paymentVnPay")]
        public async Task<IActionResult> CheckoutPaymentVnPay([FromBody] VnPayCheckout request)
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

                var findorder = await _context.Orders.FindAsync(request.Id);
                findorder.isPaid = true;
                findorder.isPaymentOnline = true;
                await _context.SaveChangesAsync();
                return Ok((new Response { Status = 200, Message = "Thanh toán thành công" }));


            }
            else return BadRequest((new Response { Status = 400, Message = "Thanh toán thất bại" }));










        }

    }
}
