using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.IService;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CactusProject.Services
{
    public class ShoppingCartService : InShoppingCartService<ShoppingCart>
    {
        private readonly CactusContext cactusContext;

        public ShoppingCartService(CactusContext cactusContext)
        {
            this.cactusContext = cactusContext;
        }

        public void IncrementCount(ShoppingCart shoppingCart, int count)
        {
            var result = cactusContext.ShoppingCarts.Include(c=>c.Cactus).FirstOrDefault(c=>c.Id.Equals(shoppingCart.Id));
            if (shoppingCart.Count < result.Cactus.Amount) 
            shoppingCart.Count += count;
        }

        public void DecrementCount(ShoppingCart shoppingCart, int count)
        {
            if (shoppingCart.Count > 1)
                shoppingCart.Count -= count;
        }

        public void Save()
        {
            cactusContext.SaveChanges();
        }

        public void Add(ShoppingCart shoppingCart)
        {
            cactusContext.Add(shoppingCart);
        }
    }
}