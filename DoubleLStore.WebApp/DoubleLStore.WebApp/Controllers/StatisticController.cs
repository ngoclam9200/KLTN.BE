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
    }
}
