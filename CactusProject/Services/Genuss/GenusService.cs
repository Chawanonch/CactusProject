using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Genuss.IService;

namespace CactusProject.Services.Genuss
{
    public class GenusService : IGenusService
    {
        private readonly CactusContext cactusContext;
        public GenusService(CactusContext cactusContext)
        {
            this.cactusContext = cactusContext;
        }

        public void GetCreateAndEdit(Genus data)
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
            var genus = cactusContext.ManyGenus.Find(Id);
            cactusContext.ManyGenus.Remove(genus);
            cactusContext.SaveChanges();
        }
    }
}
