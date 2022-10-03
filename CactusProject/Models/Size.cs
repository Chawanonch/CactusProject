using System.ComponentModel.DataAnnotations;

namespace CactusProject.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Sizename { get; set; }
    }
}
