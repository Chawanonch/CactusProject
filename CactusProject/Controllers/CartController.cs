using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services;
using CactusProject.Utility;
using CactusProject.ViewModels;
using System.Security.Claims;
namespace CactusProject.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly CactusContext cactusContext;
        private readonly ShoppingCartService shoppingCartService;
        private readonly IWebHostEnvironment webHostEnvironment;

        //start
        //ใช้ได้หลายที่
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        //end

        public CartController(CactusContext cactusContext, ShoppingCartService shoppingCartService, IWebHostEnvironment webHostEnvironment)
        {
            this.cactusContext = cactusContext;
            this.shoppingCartService = shoppingCartService;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            //ค้นหาuser
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            ShoppingCartVM shoppingCartVM = new()
            {
                OrderHeader = new(),
                ListCart = cactusContext.ShoppingCarts
                .Include(c => c.Cactus)
                    .ThenInclude(c => c.Category)
                .Include(c => c.User)
                .Where(x => x.UserId == userId).ToList(),
            };

            foreach (var cart in shoppingCartVM.ListCart)
            {
                shoppingCartVM.OrderHeader.OrderTotal += cart.Count * cart.Cactus.Price;

                var de = cart.Cactus.Price;
                cart.Cactus.Price += cart.Count * de;
                cart.Cactus.Price -= de;
            };

            #region countshop
            HttpContext.Session.SetInt32("t", cactusContext.ShoppingCarts.Count());
            #endregion

            return View(shoppingCartVM);
        }
        public IActionResult Plus(int id)
        {
            var data = cactusContext.ShoppingCarts.Find(id);
            shoppingCartService.IncrementCount(data, 1);
            shoppingCartService.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int id)
        {
            var data = cactusContext.ShoppingCarts.Find(id);
            shoppingCartService.DecrementCount(data, 1);
            shoppingCartService.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            var data = cactusContext.ShoppingCarts.Find(id);
            cactusContext.Remove(data);
            shoppingCartService.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            ShoppingCartVM = new()
            {
                OrderHeader = new(),
                ListCart = cactusContext.ShoppingCarts.Include(c => c.Cactus)
                .Where(c => c.UserId == userId),
            };

            var u = ShoppingCartVM.OrderHeader.User = cactusContext.Users.Find(userId);

            ShoppingCartVM.OrderHeader.StreetAddress = u.StreetAddress;
            ShoppingCartVM.OrderHeader.City = u.City;
            ShoppingCartVM.OrderHeader.State = u.State;
            ShoppingCartVM.OrderHeader.PostalCode = u.PostalCode;
            ShoppingCartVM.OrderHeader.Name = u.FullName;

            foreach (var item in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += item.Count * item.Cactus.Price;             
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Summary(IFormFile file)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            ShoppingCartVM.ListCart = cactusContext.ShoppingCarts.Include(c => c.Cactus)
                .Where(c => c.UserId == userId).ToList();

            var test = ShoppingCartVM;

            #region Image Management
            string wwwRootPath = webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(file.FileName);
                var uploads = Path.Combine(wwwRootPath, @"images\payments");

                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                //บันทึกรุปภาพใหม่
                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                ShoppingCartVM.OrderHeader.PaymentImage = @"\images\payments\" + fileName + extension;
            }
            #endregion


            foreach (var item in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += item.Count * item.Cactus.Price;
            }

            ShoppingCartVM.OrderHeader.UserId = userId;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;

            cactusContext.OrderHeaders.Add(ShoppingCartVM.OrderHeader);

            cactusContext.SaveChanges();

            foreach (var item in ShoppingCartVM.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    CactusId = item.CactusId,
                    Count = item.Count,
                };
                cactusContext.OrderDetails.Add(orderDetail);
				var product = await cactusContext.ManyCactus.FindAsync(item.CactusId);

				if (item.Count <= product.Amount) product.Amount -= item.Count;
				cactusContext.Update(product);
			}

            cactusContext.ShoppingCarts.RemoveRange(ShoppingCartVM.ListCart);

            cactusContext.SaveChanges();

            TempData["success"] = "successful transaction";
            return RedirectToAction("Index", "Home");

        }
    }
}