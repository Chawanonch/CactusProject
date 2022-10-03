using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Sourcess.IService;
using Microsoft.AspNetCore.Mvc;

namespace CactusProject.Controllers
{
    public class SourceController : Controller
    {
        private readonly CactusContext cactusContext;
        private readonly ISourceService sourceService;

        public SourceController(CactusContext cactusContext,ISourceService sourceService)
        {
            this.cactusContext = cactusContext;
            this.sourceService = sourceService;
        }
        public IActionResult Index()
        {
            return View(cactusContext.Sources);
        }

        public IActionResult CreateAndUpdate(int? Id)
        {
            if (Id == 0 || Id == null) return View();

            var sources = cactusContext.Sources.Find(Id);

            return View(sources);
        }
        [HttpPost]
        public IActionResult CreateAndUpdate(Source data)
        {
            if (!ModelState.IsValid) return View();

            sourceService.GetCreateAndEdit(data);
            TempData["Success"] = "Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            sourceService.GetRemove(Id);
            TempData["Success"] = "Delete Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
