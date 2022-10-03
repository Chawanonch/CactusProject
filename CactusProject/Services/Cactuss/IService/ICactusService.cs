using CactusProject.Models;
using CactusProject.ViewModels;

namespace CactusProject.Services.Cactuss.IService
{
    public interface ICactusService
    {
        Task<List<Cactus>> GetByAll(int categoryId, string search, int amount,int sortby);
        List<Cactus> GetCactus();
        CactusVM GetUpsert(int? id);

        void RemoveCactus(int id);
    }
}
