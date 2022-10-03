using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Sizes.IService;

namespace CactusProject.Services.Sizes
{
    public class SizeService : ISizeService
    {

        private readonly CactusContext cactusContext;
        public SizeService(CactusContext cactusContext)
        {
            this.cactusContext = cactusContext;
        }

        public void GetCreateAndEdit(Size data)
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
            var size = cactusContext.Sizes.Find(Id);
            cactusContext.Sizes.Remove(size);
            cactusContext.SaveChanges();
        }
    }
}
