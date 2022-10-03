using CactusProject.Models;
using CactusProject.ViewModels;

namespace CactusProject.Services.Orderss.IService
{
    public interface IOrderService
    {
        OrderVM GetDetail(int id);
        void GetRemove(int id);
    }
}
