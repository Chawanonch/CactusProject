using CactusProject.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CactusProject.ViewModels
{
    public class CactusVM
    {
        public Cactus Cactus { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SourceList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SizeList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> GenusList { get; set; }
    }
}
