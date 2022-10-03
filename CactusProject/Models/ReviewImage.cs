using System.ComponentModel.DataAnnotations;

namespace CactusProject.Models
{
    public class ReviewImage
    {
        [Key]
        public string Id { get; set; }
        public string ReviewId { get; set; }

        public string Image { get; set; }
    }
}
