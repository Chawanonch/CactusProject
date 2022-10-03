using CactusProject.Models;

namespace CactusProject.ViewModels
{
    public class ShoppingCartVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<ShoppingCart> ListCart { get; set; }

    }
}
