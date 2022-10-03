using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Sourcess.IService;

namespace CactusProject.Services.Sourcess
{
    public class SourceService : ISourceService
    {
        private readonly CactusContext cactusContext;
        public SourceService(CactusContext cactusContext)
        {
            this.cactusContext = cactusContext;
        }

        public void GetCreateAndEdit(Source data)
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
            var source = cactusContext.Sources.Find(Id);
            cactusContext.Sources.Remove(source);
            cactusContext.SaveChanges();
        }
    }
}
