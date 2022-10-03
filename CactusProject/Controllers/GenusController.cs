using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Genuss.IService;
using Microsoft.AspNetCore.Mvc;

namespace CactusProject.Controllers
{
    public class GenusController : Controller
    {
        private readonly CactusContext cactusContext;
        private readonly IGenusService genusService;

        public GenusController(CactusContext cactusContext, IGenusService genusService)
        {
            this.cactusContext = cactusContext;
            this.genusService = genusService;
        }
        public IActionResult Index()
        {
            return View(cactusContext.ManyGenus);
        }

        public IActionResult CreateAndUpdate(int? Id)
        {
            if (Id == 0 || Id == null) return View();

            var genus = cactusContext.ManyGenus.Find(Id);

            return View(genus);
        }
        [HttpPost]
        public IActionResult CreateAndUpdate(Genus data)
        {
            if (!ModelState.IsValid) return View();

            genusService.GetCreateAndEdit(data);
            TempData["Success"] = "Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            genusService.GetRemove(Id);
            TempData["Success"] = "Delete Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
