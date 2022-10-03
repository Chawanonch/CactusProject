using CactusProject.Models;

namespace CactusProject.Services.Categorys.IService
{
    public interface ICategoryService
    {
        void GetCreateAndEdit(Category data);
        void GetRemove(int Id);
    }
}
