using CactusProject.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CactusProject.ViewModels
{
    public class ReviewVM
    {
        public Review Review { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Enter number between 1 to 5")]
        public int Star { get; set; }

        public IFormFileCollection? Images { get; set; }
    }
}
