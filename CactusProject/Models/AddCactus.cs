using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CactusProject.Models
{
    public class AddCactus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Amount { get; set; }

        public DateTime DateDelivery { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Cactus")]
        public int CactusId { get; set; }

        [ForeignKey("CactusId")]
        [ValidateNever]
        public Cactus Cactus { get; set; }

    }
}
