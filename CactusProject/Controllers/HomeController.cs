using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services;
using CactusProject.Services.Cactuss.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace CactusProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly CactusContext cactusContext;
        private readonly ShoppingCartService shoppingCartService;
        private readonly ICactusService cactusService;

        public HomeController(CactusContext cactusContext, ShoppingCartService shoppingCartService, ICactusService cactusService)
        {
            this.cactusContext = cactusContext;
            this.shoppingCartService = shoppingCartService;
            this.cactusService = cactusService;
        }

        public IActionResult Aboutus()
        {
            #region countshop
            HttpContext.Session.SetInt32("t", cactusContext.ShoppingCarts.Count());
            #endregion
            return View();
        }

        public async Task<IActionResult> Index(int categoryId = 0, string search = "", int amount = 0, int sortby = 0)
        {
            List<Cactus> data;
            var product = await cactusContext.ManyCactus.ToListAsync();

            #region cateId , search
            data = await cactusService.GetByAll(categoryId, search, amount, sortby);
            #endregion

            #region count;product,roop;cate,shop;count using viewbag-sesion
            if (product.Count > 0)
            {
                ViewBag.count = data.Count();
            }
            else
            {
                ViewBag.count = 0;
            }

            if (categoryId != null)
            {
                ViewBag.Cate = await cactusContext.Categories.ToListAsync();
            }
            
            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.searchpc = "ค้นหา : " + search;
            }

            HttpContext.Session.SetInt32("t", cactusContext.ShoppingCarts.Count());
            #endregion

            #region random
            var c = new Random(1);
            ViewBag.random = c.NextDouble();
            #endregion

            return View(data);
        }

        public async Task<IActionResult> Details(int productId, int categoryId)
        {
            #region Star + Countreview
            var re = await cactusContext.Reviews.Where(c => c.CactusId == productId).ToListAsync();

            foreach (var review in re)
            {
                if (review.Id != null)
                {
                    ViewBag.Avg = re.Average(c => c.Star).ToString("#.#");
                    ViewBag.review = await cactusContext.Reviews.Where(c => c.CactusId == productId).CountAsync();
                }
            }
            #endregion

            #region random
            var c = new Random(1);
            ViewBag.random = c.NextDouble();
            #endregion

            if (productId > 0 && categoryId > 0)
            {
                ViewBag.Showproduct = await cactusContext.ManyCactus.Where(c => c.Id != productId && c.CategoryId != categoryId).ToListAsync();
                ViewBag.Showproductcate = await cactusContext.ManyCactus.Where(c => c.Id != productId && c.CategoryId.Equals(categoryId)).ToListAsync();
            }

            #region shoppingcart
            var data = new ShoppingCart
            {
                CactusId = productId,
                Count = 1,
                Cactus = cactusContext.ManyCactus
                .Include(c => c.Category)
                .Include(c => c.Source)
                .Include(c => c.Genus)
                .Include(c => c.Size)
                .Include(c => c.Image)
                .Include(c => c.Review)
                    .ThenInclude(c => c.ReviewImage)
                .Include(c => c.Review)
                    .ThenInclude(c => c.User)
                .FirstOrDefault(c => c.Id.Equals(productId)),

            };
            #endregion

            return View(data);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.UserId = claim.Value; //ใครทำรายการ

            var oldCart = cactusContext.ShoppingCarts.Include(c => c.Cactus).FirstOrDefault(c => c.UserId.Equals(shoppingCart.UserId) && c.CactusId.Equals(shoppingCart.CactusId));

            if (oldCart == null) //new
            {
                shoppingCartService.Add(shoppingCart);
            }
            else //update
            {
                shoppingCartService.IncrementCount(oldCart, shoppingCart.Count);
            }

            shoppingCartService.Save();
            TempData["Success"] = "อัพเดทเสร็จสิน";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}