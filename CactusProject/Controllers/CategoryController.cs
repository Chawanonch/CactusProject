using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Categorys.IService;
using Microsoft.AspNetCore.Mvc;

namespace CactusProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CactusContext cactusContext;
        private readonly ICategoryService categoryService;

        public CategoryController(CactusContext cactusContext,ICategoryService categoryService)
        {
            this.cactusContext = cactusContext;
            this.categoryService = categoryService;
        }
        public IActionResult Index()
        {
            return View(cactusContext.Categories);
        }

        public IActionResult CreateAndUpdate(int? Id)
        {
            if (Id == 0 || Id == null) return View();

            var category = cactusContext.Categories.Find(Id);

            return View(category);
        }

        [HttpPost]
        public IActionResult CreateAndUpdate(Category data)
        {
            if (!ModelState.IsValid) return View();

            categoryService.GetCreateAndEdit(data);
            TempData["Success"] = "Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            categoryService.GetRemove(Id);
            TempData["Success"] = "Delete Successfully";
            return RedirectToAction(nameof(Index));
        }

    }
}
