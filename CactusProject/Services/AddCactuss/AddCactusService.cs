using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.AddCactuss.IService;
using CactusProject.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CactusProject.Services.AddCactuss
{
    public class AddCactusService : IAddCactusService
    {
        private readonly CactusContext cactusContext;

        public AddCactusService(CactusContext cactusContext)
        {
            this.cactusContext = cactusContext;
        }
        public AddVM GetAdd()
        {
            AddVM addVM = new()
            {
                AddCactus = new(),
                CactusList = cactusContext.ManyCactus.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
            };
            return addVM;
        }
    }
}
