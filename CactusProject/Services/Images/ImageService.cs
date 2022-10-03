using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Images.IService;
using CactusProject.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CactusProject.Services.Images
{
    public class ImageService : IImageService
    {

        private readonly CactusContext cactusContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImageService(CactusContext cactusContext, IWebHostEnvironment webHostEnvironment)
        {
            this.cactusContext = cactusContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        public List<Image> GetImage()
        {
            //Include คือจอยกับclassไหน
            return cactusContext.Images.Include(c => c.Cactus).ToList();
        }

        public ImageVM GetUpImage(int? id)
        {
            ImageVM imageVM = new()
            {
                Image = new(),
                CactusList = cactusContext.ManyCactus.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            if (id == 0 || id == null)
            {
            }
            else
            {
                imageVM.Image = cactusContext.Images.Find(id);
            }

            return imageVM;
        }

        public void RemoveImage(int id)
        {
            var data = cactusContext.Images.Find(id);

            string wwwRootPath = webHostEnvironment.WebRootPath;
            var uploads = Path.Combine(wwwRootPath, @"images\products");

            if (data.Name != null)
            {
                var oldImagePath = Path.Combine(wwwRootPath, data.Name.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            cactusContext.Remove(data);
            cactusContext.SaveChanges();
        }
    }
}
