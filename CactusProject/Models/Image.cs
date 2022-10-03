using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CactusProject.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Cactus Name")]
        public int CactusId { get; set; }

        [ForeignKey("CactusId")]
        [ValidateNever]
        public Cactus Cactus { get; set; }
    }
}
