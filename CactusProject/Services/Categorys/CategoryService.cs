using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Categorys.IService;

namespace CactusProject.Services.Categorys
{
    public class CategoryService : ICategoryService
    {
        private readonly CactusContext cactusContext;

        public CategoryService(CactusContext cactusContext)
        {
            this.cactusContext = cactusContext;
        }
        
        public void GetCreateAndEdit(Category data)
        {
            //ใช้createกับeditเหมือนกัน
            if (data.Id == 0 || data.Id == null)
            {
                cactusContext.Add(data);
            }
            else
            {
                cactusContext.Update(data);
            }
            cactusContext.SaveChanges();
        }

        public void GetRemove(int Id)
        {
            var category = cactusContext.Categories.Find(Id);
            cactusContext.Categories.Remove(category);
            cactusContext.SaveChanges();
        }
    }
}
