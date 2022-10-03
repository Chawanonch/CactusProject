using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CactusProject.Models
{
    public class Review
    {
        public Review()
        {
            ReviewImage = new HashSet<ReviewImage>();
        }

        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public int CactusId { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        public int Star { get; set; }

        public DateTime? CreateDate { get; set; }

        [ValidateNever]
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ValidateNever]
        [ForeignKey("CactusId")]
        public Cactus Cactus { get; set; }

        [ValidateNever]
        public ICollection<ReviewImage> ReviewImage { get; set; }
    }
}
