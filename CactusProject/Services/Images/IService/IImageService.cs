using CactusProject.Models;
using CactusProject.ViewModels;

namespace CactusProject.Services.Images.IService
{
    public interface IImageService
    {
        List<Image> GetImage();
        ImageVM GetUpImage(int? id);

        void RemoveImage(int id);
    }
}
