using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Sizes.IService;
using Microsoft.AspNetCore.Mvc;

namespace CactusProject.Controllers
{
    public class SizeController : Controller
    {
        private readonly CactusContext cactusContext;
        private readonly ISizeService sizeService;

        public SizeController(CactusContext cactusContext,ISizeService sizeService)
        {
            this.cactusContext = cactusContext;
            this.sizeService = sizeService;
        }
        public IActionResult Index()
        {
            return View(cactusContext.Sizes);
        }

        public IActionResult CreateAndUpdate(int? Id)
        {
            if (Id == 0 || Id == null) return View();

            var size = cactusContext.Sizes.Find(Id);

            return View(size);
        }
        [HttpPost]
        public IActionResult CreateAndUpdate(Size data)
        {
            if (!ModelState.IsValid) return View();

            sizeService.GetCreateAndEdit(data);
            TempData["Success"] = "Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            sizeService.GetRemove(Id);
            TempData["Success"] = "Delete Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
