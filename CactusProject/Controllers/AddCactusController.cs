using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.AddCactuss.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CactusProject.Controllers
{
    public class AddCactusController : Controller
    {
        private readonly CactusContext cactusContext;
        private readonly IAddCactusService addCactusService;


        public AddCactusController(CactusContext cactusContext, IAddCactusService addCactusService)
        {
            this.cactusContext = cactusContext;
            this.addCactusService = addCactusService;
        }
        public IActionResult HomeAS()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var data = await cactusContext.AddManyCactus.Include(c => c.Cactus).OrderBy(c=>c.DateDelivery.Day).ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> AddStock()
        {
            var data = addCactusService.GetAdd();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddStock(AddCactus addcactus)
        {
            var result = await cactusContext.ManyCactus.FirstOrDefaultAsync(c => c.Id.Equals(addcactus.CactusId));

            if (addcactus.Amount > 0)
            {
                await cactusContext.AddManyCactus.AddAsync(addcactus);
                result.Amount += addcactus.Amount;
                result.Date = DateTime.Now;
                cactusContext.ManyCactus.Update(result);
                await cactusContext.SaveChangesAsync();

                TempData["Success"] = "เพิ่มจำนวนสินค้าเสร็จสิน";
            }
            else
            {
                TempData["Error"] = "สินค้าเกินจำนวนไม่สามารถทำรายการได้";
            }
            return RedirectToAction("AddStock", "AddCactus");
        }

        public async Task<IActionResult> Delete(int? id, AddCactus addcactus) /*AddCactus addcactus*/
        {
            var result = await cactusContext.AddManyCactus.FindAsync(id);

            #region ลบแล้วจำนวนหาย
            var data = await cactusContext.ManyCactus.FirstOrDefaultAsync(c => c.Id.Equals(addcactus.CactusId));

            if (addcactus.Amount <= data.Amount)
            {
                cactusContext.AddManyCactus.Remove(result);

                data.Amount -= addcactus.Amount;
                data.Date = DateTime.Now;
                cactusContext.ManyCactus.Update(data);
                TempData["Success"] = "ลบเสร็จสิน";
                await cactusContext.SaveChangesAsync();

                return RedirectToAction("Index", "AddCactus");
            }
            else
            {
                TempData["Error"] = "ของมีจำนวนน้อยกว่าที่ต้องการลบ";
            }
            #endregion

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveAllRecord()
        {
            cactusContext.AddManyCactus.RemoveRange(await cactusContext.AddManyCactus.ToListAsync());

            await cactusContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
