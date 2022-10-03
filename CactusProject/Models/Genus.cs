using System.ComponentModel.DataAnnotations;

namespace CactusProject.Models
{
    public class Genus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
