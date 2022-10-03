using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Images.IService;
using CactusProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CactusProject.Controllers
{
    public class ImageController : Controller
    {
        private readonly CactusContext cactusContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IImageService imageService;

        public ImageController(CactusContext cactusContext, IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            this.cactusContext = cactusContext;
            this.webHostEnvironment = webHostEnvironment;
            this.imageService = imageService;
        }
        public IActionResult Index()
        {
            return View(imageService.GetImage());
        }

        public IActionResult UpSert(int? id)
        {
            return View(imageService.GetUpImage(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(ImageVM data, IFormFile file,Cactus cactus)
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
                    if (data.Image.Name != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, data.Image.Name.TrimStart('\\'));
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
                    data.Image.Name = @"\images\products\" + fileName + extension;
                }
            }
            if (data.Image.Id == 0)
                cactusContext.Images.Add(data.Image);
            else
                cactusContext.Images.Update(data.Image);
            cactusContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            imageService.RemoveImage(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
