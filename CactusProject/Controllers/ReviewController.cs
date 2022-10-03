using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Revieww.IService;
using CactusProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CactusProject.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService reviewService;
        private readonly CactusContext cactusContext;

        public ReviewController(IReviewService reviewService,CactusContext cactusContext)
        {
            this.reviewService = reviewService;
            this.cactusContext = cactusContext;
        }

        public IActionResult Index(string id)
        {
            if(id == null) return View();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ReviewVM reviewVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            reviewVM.UserId = claim.Value; //ใครทำรายการ

            string msg = await reviewService.CreateReview(reviewVM);

            if (msg == "Success")
            {
                TempData["Success"] = "สร้างเสร็จสิน";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Success"] = msg;
            }
            return View(msg);
        }

        public async Task<IActionResult> RemoveReview(string id)
        {
            var result = await cactusContext.Reviews.FindAsync(id);
            cactusContext.Reviews.Remove(result);

            await cactusContext.SaveChangesAsync();

            TempData["Success"] = "ลบเสร็จสิน";
            return RedirectToAction("Index", "Home");
        }
    }
}
