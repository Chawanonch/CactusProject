using CactusProject.Data;
using CactusProject.Services.Cactuss.IService;
using CactusProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CactusProject.Controllers
{
    public class CactusController : Controller
    {
        private readonly CactusContext cactusContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICactusService cactusService;

        public CactusController(CactusContext cactusContext, IWebHostEnvironment webHostEnvironment,ICactusService cactusService)
        {
            this.cactusContext = cactusContext;
            this.webHostEnvironment = webHostEnvironment;
            this.cactusService = cactusService;
        }
        public IActionResult Index()
        {
            return View(cactusService.GetCactus());
        }

        public IActionResult UpSert(int? id)
        {
            return View(cactusService.GetUpsert(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(CactusVM data, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    //สุ่มชื่อสุ่มไฟล์
                    string fileName = Guid.NewGuid().ToString();
                    //เช็คนามสกุลไฟล์ชื่อ
                    var extension = Path.GetExtension(file.FileName);
                    //เก็บรูปไว้ในไหน
                    var uploads = Path.Combine(wwwRootPath, @"images\products");


                    if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                    //กรณีมีรูปภาพเดิมต้องลบทิ้งก่อน
                    if (data.Cactus.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, data.Cactus.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    //บันทึกรุปภาพใหม่
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    data.Cactus.ImageUrl = @"\images\products\" + fileName + extension;
                }
            }
            if (data.Cactus.Id != 0)
            {
                cactusContext.ManyCactus.Update(data.Cactus);
                TempData["Success"] = "อัพเดทเสร็จสิน";
            }
            else
            {
                cactusContext.ManyCactus.Add(data.Cactus);
                TempData["Success"] = "สร้างเสร็จสิน";
            }

            cactusContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            cactusService.RemoveCactus(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
