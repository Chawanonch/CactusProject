using CactusProject.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CactusProject.ViewModels
{
    public class AddVM
    {
        [ValidateNever]
        public AddCactus AddCactus { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CactusList { get; set; }

    }
}
