using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CactusProject.Models
{
    public class ShoppingCart
    {

        [Key]
        public int Id { get; set; }

        public int CactusId { get; set; }

        [ForeignKey("CactusId")]
        [ValidateNever]
        public Cactus Cactus { get; set; }

        [Range(1, 1000, ErrorMessage = "กรุณากรอก {1} ไม่เกิน {2}")]
        public int Count { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }

    }
}
