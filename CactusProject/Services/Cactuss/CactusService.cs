using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Cactuss.IService;
using CactusProject.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CactusProject.Services.Cactuss
{
    public class CactusService : ICactusService
    {
        private readonly CactusContext cactusContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CactusService(CactusContext cactusContext, IWebHostEnvironment webHostEnvironment)
        {
            this.cactusContext = cactusContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Cactus>> GetByAll(int categoryId, string search, int amount, int sortby)
        {
            List<Cactus> data;

            var dataAll = await cactusContext.ManyCactus.Include(c => c.Category).ToListAsync();

            if (categoryId > 0 || !string.IsNullOrEmpty(search) || amount > 0 || sortby > 0)
            {
                if(categoryId > 0)
                {
                    data = await cactusContext.ManyCactus.Where(A => A.CategoryId.Equals(categoryId)).ToListAsync();
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    data = await cactusContext.ManyCactus.Where(a => a.Name.Contains(search) || a.Category.Name.Equals(search)).ToListAsync();
                }

                #region amount
                else if (amount > 0)
                {
                    if (amount == 1)
                    {
                        data = await cactusContext.ManyCactus.Where(a => a.Amount <= 25 && a.Amount >= 1).ToListAsync();
                    }
                    else if (amount == 2)
                    {
                        data = await cactusContext.ManyCactus.Where(a => a.Amount == 0).ToListAsync();
                    }
                    else
                    {
                        data = dataAll;
                    }
                }
                #endregion

                #region sortby
                else if (sortby > 0)
                {
                    if (sortby == 1)
                    {
                        data = await cactusContext.ManyCactus.OrderBy(c => c.Price).ToListAsync();

                    }
                    else if (sortby == 2)
                    {
                        data = await cactusContext.ManyCactus.OrderByDescending(c => c.Price).ToListAsync();
                    }
                    else if (sortby == 3)
                    {
                        data = await cactusContext.ManyCactus.OrderBy(c => c.Name).ToListAsync();
                    }
                    else if (sortby == 4)
                    {
                        data = await cactusContext.ManyCactus.OrderByDescending(c => c.Name).ToListAsync();
                    }
                    else if (sortby == 5)
                    {
                        data = await cactusContext.ManyCactus.OrderByDescending(c => c.Date).ToListAsync();
                    }
                    else if (sortby == 6)
                    {
                        data = await cactusContext.ManyCactus.OrderBy(c => c.Date).ToListAsync();
                    }
                    else
                    {
                        data = dataAll;
                    }
                }
                #endregion

                else
                {
                    data = dataAll;
                }
            }
            else
            {
                data = dataAll;
            }

            return data;
        }
        //public async Task<List<Cactus>> GetByAll(int categoryId, string search, int amount,int sortby)
        //{
        //    List<Cactus> data;

        //    if (categoryId > 0)
        //    {
        //        data = await cactusContext.ManyCactus.Where(A => A.CategoryId.Equals(categoryId)).ToListAsync();
        //    }
        //    else if (!string.IsNullOrEmpty(search))
        //    {
        //        data = await cactusContext.ManyCactus.Where(a => a.Name.Contains(search)).ToListAsync();
        //    }

        //    #region amount
        //    else if (amount > 0)
        //    {
        //        if(amount == 1)
        //        {
        //            data = await cactusContext.ManyCactus.Where(a => a.Amount <= 25 && a.Amount >= 1).ToListAsync();
        //        }
        //        else if(amount == 2)
        //        {
        //            data = await cactusContext.ManyCactus.Where(a => a.Amount == 0).ToListAsync();
        //        }
        //        else
        //        {
        //            data = await cactusContext.ManyCactus.Include(c => c.Category).ToListAsync();
        //        }
        //    }
        //    #endregion

        //    #region sortby
        //    else if (sortby > 0)
        //    {
        //        if (sortby == 1)
        //        {
        //            data = await cactusContext.ManyCactus.OrderBy(c => c.Price).ToListAsync();

        //        }
        //        else if (sortby == 2)
        //        {
        //            data = await cactusContext.ManyCactus.OrderByDescending(c => c.Price).ToListAsync();
        //        }
        //        else if (sortby == 3)
        //        {
        //            data = await cactusContext.ManyCactus.OrderBy(c => c.Name).ToListAsync();
        //        }
        //        else if (sortby == 4)
        //        {
        //            data = await cactusContext.ManyCactus.OrderByDescending(c=>c.Name).ToListAsync();
        //        }
        //        else if (sortby == 5)
        //        {
        //            data = await cactusContext.ManyCactus.OrderByDescending(c => c.Date).ToListAsync();
        //        }
        //        else if (sortby == 6)
        //        {
        //            data = await cactusContext.ManyCactus.OrderBy(c => c.Date).ToListAsync();
        //        }
        //        else
        //        {
        //            data = await cactusContext.ManyCactus.Include(c => c.Category).ToListAsync();
        //        }
        //    }
        //    #endregion

        //    else
        //    {
        //        data = await cactusContext.ManyCactus.Include(c => c.Category).ToListAsync();
        //    }

        //    return data;
        //}

        public List<Cactus> GetCactus()
        {
            //Include คือจอยกับclassไหน
            return cactusContext.ManyCactus.Include(c => c.Category).Include(c => c.Source).Include(c => c.Genus).Include(c => c.Size).ToList();
        }

        public CactusVM GetUpsert(int? id)
        {
            CactusVM cactusVM = new()
            {
                Cactus = new(),
                CategoryList = cactusContext.Categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                SourceList = cactusContext.Sources.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                GenusList = cactusContext.ManyGenus.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                SizeList = cactusContext.Sizes.Select(c => new SelectListItem
                {
                    Text = c.Sizename,
                    Value = c.Id.ToString()
                })
            };

            if (id == 0 || id == null)
            {
            }
            else
            {
                var data = cactusContext.ManyCactus.Find(id);
                cactusVM.Cactus = data;
            }

            return cactusVM;
        }

        public void RemoveCactus(int id)
        {
            var data = cactusContext.ManyCactus.Find(id);

            string wwwRootPath = webHostEnvironment.WebRootPath;
            var uploads = Path.Combine(wwwRootPath, @"images\products");

            if (data.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(wwwRootPath, data.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            cactusContext.Remove(data);
            cactusContext.SaveChanges();
        }
    }
}
