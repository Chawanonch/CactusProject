using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Orderss.IService;
using CactusProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CactusProject.Services.Orderss
{
    public class OrderService : IOrderService
    {
        private readonly CactusContext cactusContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderService(CactusContext cactusContext, IWebHostEnvironment webHostEnvironment)
        {
            this.cactusContext = cactusContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        public OrderVM GetDetail(int id)
        {
            OrderVM ordervm = new()
            {
                OrderHeader = cactusContext.OrderHeaders.Include(c => c.User).FirstOrDefault(c => c.Id == id),
                OrderDetail = cactusContext.OrderDetails.Include(c => c.Cactus).Where(x => x.OrderId == id).ToList()
            };
            return ordervm;
        }

        public void GetRemove(int id)
        {
            //ลบตัวแม่
            var data = cactusContext.OrderHeaders.Find(id);
            cactusContext.Remove(data);

            string wwwRootPath = webHostEnvironment.WebRootPath;
            var uploads = Path.Combine(wwwRootPath, @"images\payments");

            if (data.PaymentImage != null)
            {
                var oldImagePath = Path.Combine(wwwRootPath, data.PaymentImage.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            cactusContext.SaveChanges();
        }
    }
}
