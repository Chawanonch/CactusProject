using CactusProject.Data;
using CactusProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CactusProject.Controllers
{
    public class GraphController : Controller
    {
        private readonly CactusContext cactusContext;

        public GraphController(CactusContext cactusContext)
        {
            this.cactusContext = cactusContext;
        }
        public async Task<IActionResult> Graph0(string status, DateTime? date = null)
        {
            List<OrderHeader> data;
            if (date != null)
            {
                data = await cactusContext.OrderHeaders
                    .Where(e => e.PaymentDate.Day.Equals(Convert.ToDateTime(date).Day) && e.PaymentDate.Month.Equals(Convert.ToDateTime(date).Month))
                    .ToListAsync();
            }
            else
            {
                data = await cactusContext.OrderHeaders.ToListAsync();
            }

            var sum = data.Where(c => c.OrderStatus == status).Sum(c => c.OrderTotal);

            List<DataPoint> dataPoints = new List<DataPoint>();

            foreach (var item in data)
            {
                if(status == item.OrderStatus)
                {
                    dataPoints.Add(new DataPoint(item.PaymentDate.ToString("dd/MM/yyyy"), (item.OrderTotal / sum) * 100, item.OrderTotal));
                }
            }

            if (sum > 0)
            {
                ViewBag.sumtotal = sum;
            }
            else
            {
                ViewBag.sumtotal = 0;
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }

        public async Task<IActionResult> GraphOrder(string status = "Approved", DateTime? date = null)
        {
            await Graph0(status, date);
            return View();
        }

        public async Task<IActionResult> GraphOrder1(string status = "Approved", DateTime? date = null)
        {
            await Graph0(status, date);
            return View();
        }
        public async Task<IActionResult> GraphOrder2(string status = "Approved", DateTime? date = null)
        {
            await Graph0(status, date);
            return View();
        }
        public async Task<IActionResult> GraphOrder3(string status = "Approved", DateTime? date = null)
        {
            await Graph0(status, date);
            return View();
        }
        public async Task<IActionResult> GraphOrder4(string status = "Approved", DateTime? date = null)
        {
            await Graph0(status, date);
            return View();
        }
    }
}
