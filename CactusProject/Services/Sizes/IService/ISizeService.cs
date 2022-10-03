using CactusProject.Models;

namespace CactusProject.Services.Sizes.IService
{
    public interface ISizeService
    {
        void GetCreateAndEdit(Size data);
        void GetRemove(int Id);
    }
}
