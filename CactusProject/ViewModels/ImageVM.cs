using CactusProject.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CactusProject.ViewModels
{
    public class ImageVM
    {
        [ValidateNever]
        public Image Image { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CactusList { get; set; }
    }
}
