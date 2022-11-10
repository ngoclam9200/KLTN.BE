using DoubleLStore.WebApp.Common;
using DoubleLStore.WebApp.EF;
using DoubleLStore.WebApp.IService;
using DoubleLStore.WebApp.ViewModels.Statistic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DoubleLStore.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly doubleLStoreDbContext _context;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public StatisticController(doubleLStoreDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;

        }
        [HttpGet("get-turnover")]

        public async Task<IActionResult> GetTurnOver()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var finorder = await _context.Orders.Where(x => x.StatusOrderId == "3").ToListAsync();
            ArrayList listdata = new ArrayList();
            for (int j = 0; j < 12; j++)
            {

                var total = 0;
                for (int i = 0; i < finorder.Count; i++)
                {

                    if (finorder[i].OrderDate.Month == currentMonth && finorder[i].OrderDate.Year == currentYear)
                    {
                        total += Convert.ToInt32(finorder[i].UnitPrice);
                    }

                }
                Turnover turnovver = new Turnover();
                turnovver.Total= total;
                turnovver.monthyear = currentMonth.ToString() + "/" + currentYear;
                listdata.Add(turnovver);
                currentMonth = currentMonth - 1;

                if (currentMonth == 0)
                {
                    currentMonth = 12;
                    currentYear = currentYear - 1;
                }

             }    
            

             
            return Ok(new Response { Status = 200, Message = "Success", Data = listdata });

        }
        [HttpGet("get-cost-salary")]

        public async Task<IActionResult> GetCostSalary()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var monthyear=currentMonth.ToString() +"/" + currentYear.ToString();
            var finsalary = await _context.SalaryStaffs.Where(x => x.isWorking == false).ToListAsync();
            ArrayList listdata = new ArrayList();
            for (int j = 0; j < 12; j++)
            {

                var total = 0;
                for (int i = 0; i < finsalary.Count; i++)
                {

                    if (finsalary[i].Month == monthyear)
                    {
                        total += Convert.ToInt32(finsalary[i].SalaryOfThisMonth);
                    }

                }
                Turnover turnovver = new Turnover();
                turnovver.Total = total;
                turnovver.monthyear = monthyear;
                listdata.Add(turnovver);
                currentMonth = currentMonth - 1;

                if (currentMonth == 0)
                {
                    currentMonth = 12;
                    currentYear = currentYear - 1;
                }
                monthyear = currentMonth.ToString() + "/" + currentYear.ToString();

            }



            return Ok(new Response { Status = 200, Message = "Success", Data = listdata });

        }
        [HttpGet("get-cost-product")]

        public async Task<IActionResult> GetCostProduct()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var monthyear = currentMonth.ToString() + "/" + currentYear.ToString();
            var findcostprod = await _context.CostProducts.ToListAsync();
            ArrayList listdata = new ArrayList();
            for (int j = 0; j < 12; j++)
            {

                var total = 0;
                for (int i = 0; i < findcostprod.Count; i++)
                {

                    if (findcostprod[i].Month == monthyear)
                    {
                        total += Convert.ToInt32(findcostprod[i].TotalCost);
                    }

                }
                Turnover turnovver = new Turnover();
                turnovver.Total = total;
                turnovver.monthyear = monthyear;
                listdata.Add(turnovver);
                currentMonth = currentMonth - 1;

                if (currentMonth == 0)
                {
                    currentMonth = 12;
                    currentYear = currentYear - 1;
                }
                monthyear = currentMonth.ToString() + "/" + currentYear.ToString();

            }



            return Ok(new Response { Status = 200, Message = "Success", Data = listdata });

        }
        [HttpGet("get-cost-voucher")]

        public async Task<IActionResult> GetCostVoucher()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var findcostvoucher = await _context.CostVouchers.ToListAsync();
            ArrayList listdata = new ArrayList();
            for (int j = 0; j < 12; j++)
            {

                var total = 0;
                for (int i = 0; i < findcostvoucher.Count; i++)
                {

                    if (findcostvoucher[i].DateCreated.Month == currentMonth && findcostvoucher[i].DateCreated.Year == currentYear)
                    {
                        total += Convert.ToInt32(findcostvoucher[i].Cost);
                    }

                }
                Turnover turnovver = new Turnover();
                turnovver.Total = total;
                turnovver.monthyear = currentMonth.ToString() + "/" + currentYear;
                listdata.Add(turnovver);
                currentMonth = currentMonth - 1;

                if (currentMonth == 0)
                {
                    currentMonth = 12;
                    currentYear = currentYear - 1;
                }

            }



            return Ok(new Response { Status = 200, Message = "Success", Data = listdata });

        }
        [HttpGet("get-count-delivered")]

        public async Task<IActionResult> GetCountDelivered()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var finorder = await _context.Orders.Where(x => x.StatusOrderId == "3").ToListAsync();
            ArrayList listdata = new ArrayList();
            for (int j = 0; j < 12; j++)
            {

                var total = 0;
                for (int i = 0; i < finorder.Count; i++)
                {

                    if (finorder[i].OrderDate.Month == currentMonth && finorder[i].OrderDate.Year == currentYear)
                    {
                        total += 1;
                    }

                }
                CountOrder count = new CountOrder();
                count.Count = total;
                count.monthyear = currentMonth.ToString() + "/" + currentYear;
                listdata.Add(count);
                currentMonth = currentMonth - 1;

                if (currentMonth == 0)
                {
                    currentMonth = 12;
                    currentYear = currentYear - 1;
                }

            }



            return Ok(new Response { Status = 200, Message = "Success", Data = listdata });

        }
        [HttpGet("get-count-cancel")]

        public async Task<IActionResult> GetCountCancel()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var finorder = await _context.Orders.Where(x => x.StatusOrderId == "4").ToListAsync();
            ArrayList listdata = new ArrayList();
            for (int j = 0; j < 12; j++)
            {

                var total = 0;
                for (int i = 0; i < finorder.Count; i++)
                {

                    if (finorder[i].OrderDate.Month == currentMonth && finorder[i].OrderDate.Year == currentYear)
                    {
                        total += 1;
                    }

                }
                CountOrder count = new CountOrder();
                count.Count = total;
                count.monthyear = currentMonth.ToString() + "/" + currentYear;
                listdata.Add(count);
                currentMonth = currentMonth - 1;

                if (currentMonth == 0)
                {
                    currentMonth = 12;
                    currentYear = currentYear - 1;
                }

            }



            return Ok(new Response { Status = 200, Message = "Success", Data = listdata });

        }
        [HttpGet("get-all-count-order")]

        public async Task<IActionResult> GetCountCurrentOrder()
        {
            
         
            ArrayList listdata = new ArrayList();
            for (int j = 1; j < 5; j++)
            {

              
                var finorder = await _context.Orders.Where(x=>x.StatusOrderId==j.ToString()).ToListAsync();
                var total = finorder.Count;
                listdata.Add(total);
               
            }



            return Ok(new Response { Status = 200, Message = "Success", Data = listdata });

        }
        [HttpGet("get-new-customer")]

        public async Task<IActionResult> GetNewCustomer()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var users = await _context.Users.Where(x => x.isDeleted == false).ToListAsync();
            ArrayList listdata = new ArrayList();
            for (int j = 0; j < 12; j++)
            {

                var total = 0;
                for (int i = 0; i < users.Count; i++)
                {

                    if (users[i].DateCreated.Month == currentMonth && users[i].DateCreated.Year == currentYear)
                    {
                        total += 1;
                    }

                }
                Turnover turnovver = new Turnover();
                turnovver.Total = total;
                turnovver.monthyear = currentMonth.ToString() + "/" + currentYear;
                listdata.Add(turnovver);
                currentMonth = currentMonth - 1;

                if (currentMonth == 0)
                {
                    currentMonth = 12;
                    currentYear = currentYear - 1;
                }

            }



            return Ok(new Response { Status = 200, Message = "Success", Data = listdata });

        }
        [HttpGet("get-count-product-by-category")]

        public async Task<IActionResult> GetCountProductByCategory()
        {

            var category=await _context.Categories.Where(x=>x.isDeleted==false).ToListAsync();
            var product = await _context.Products.Where(x => x.isDeleted == false).ToListAsync();
            ArrayList listdata = new ArrayList();
            for (int j = 0; j < category.Count; j++)
            {

                var total=0;
                for (int i= 0; i < product.Count; i++)
                {
                    if (category[j].Id == product[i].CategoryId)
                        total++;
                }
                CountCategory countCategory = new CountCategory();
                countCategory.Count = total;
                countCategory.CategoryName = category[j].Name;
                listdata.Add(countCategory);

            }



            return Ok(new Response { Status = 200, Message = "Success", Data = listdata });

        }
        [HttpGet("get-count-product-sold-by-category")]

        public async Task<IActionResult> GetCountProductSoldByCategory()
        {

            var category = await _context.Categories.Where(x => x.isDeleted == false).ToListAsync();
            var orderdetail = await _context.OrderDetails.ToListAsync();
            ArrayList listdata = new ArrayList();
            for (int j = 0; j < category.Count; j++)
            {

                var total = 0;
                for (int i = 0; i < orderdetail.Count; i++)
                {
                    var findorder= await _context.Orders.Where(x=>x.Id==orderdetail[i].OrderId).ToListAsync();
                    if (findorder[0].StatusOrderId=="3")
                    {
                        var findprod = await _context.Products.Where(x=>x.Id==orderdetail[i].ProductId).ToListAsync();
                        if(findprod[0].CategoryId==category[j].Id)
                          total++;

                    }    
                       
                }
                CountCategory countCategory = new CountCategory();
                countCategory.Count = total;
                countCategory.CategoryName = category[j].Name;
                listdata.Add(countCategory);

            }



            return Ok(new Response { Status = 200, Message = "Success", Data = listdata });

        }
    }
}
