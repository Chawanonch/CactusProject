using Microsoft.AspNetCore.Mvc;
using CactusProject.Data;
using CactusProject.Utility;
using CactusProject.ViewModels;
using CactusProject.Services.Orderss.IService;
using CactusProject.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CactusProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly CactusContext cactusContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IOrderService orderService;

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(CactusContext cactusContext, IWebHostEnvironment webHostEnvironment, IOrderService orderService)
        {
            this.cactusContext = cactusContext;
            this.webHostEnvironment = webHostEnvironment;
            this.orderService = orderService;
        }
        public IActionResult Index()
        {
            var data = cactusContext.OrderHeaders.ToList();

            return View(data);
        }

        public IActionResult Details(int id)
        {
            return View(orderService.GetDetail(id));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateOrderHeader()
        {
            var data = cactusContext.OrderHeaders.Find(OrderVM.OrderHeader.Id);
            var o = OrderVM.OrderHeader;

            if (data.OrderStatus == SD.StatusPending)
            {
                data.Name = o.Name;
                data.StreetAddress = o.StreetAddress;
                data.City = o.City;
                data.State = o.State;
                data.PostalCode = o.PostalCode;

                TempData["Success"] = "อัพเดทเสร็จสิน";
            }
            else
            {
                TempData["Error"] = "ไม่สามารถอัพเดทได้";
            }
            cactusContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> StatusOrder(string status)
        {

            var data = await cactusContext.OrderHeaders.FindAsync(OrderVM.OrderHeader.Id);

            if (data.OrderStatus == SD.StatusPending)
            {
                data.OrderStatus = status; //ตัวขึ้นสถานะ StatusApproved,StatusCancelled
                await cactusContext.SaveChangesAsync();

                TempData["Success"] = "อัพเดทเสร็จสิน";

            }
            else
            {
                TempData["Error"] = "ไม่สามารถอัพเดทได้";
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            orderService.GetRemove(id);
            TempData["Success"] = "Deleted Successsfully";
            return RedirectToAction(nameof(Index));
        }

    }
}

