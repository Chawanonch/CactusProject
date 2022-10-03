using CactusProject.Models;

namespace CactusProject.Services.Genuss.IService
{
    public interface IGenusService
    {
        void GetCreateAndEdit(Genus data);
        void GetRemove(int Id);
    }
}
