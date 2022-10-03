using CactusProject.Models;

namespace CactusProject.Services.Sourcess.IService
{
    public interface ISourceService
    {
        void GetCreateAndEdit(Source data);
        void GetRemove(int Id);
    }
}
