
using CactusProject.Models;

namespace CactusProject.Services.IService
{
    public interface InShoppingCartService<T>
    {
        void IncrementCount(T shoppingCart, int count);
        void DecrementCount(T shoppingCart, int count);
        void Save();
        void Add(T shoppingCart);
    }
}
