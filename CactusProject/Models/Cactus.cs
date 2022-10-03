using CactusProject.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CactusProject.Models
{
    public class Cactus
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

 
        public double Amount { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Source Name")]
        public int SourceId { get; set; }

        [ForeignKey("SourceId")]
        [ValidateNever]
        public Source Source { get; set; }

        [Required]
        [Display(Name = "Genus Name")]
        public int GenusId { get; set; }

        [ForeignKey("GenusId")]
        [ValidateNever]
        public Genus Genus { get; set; }

        [Required]
        [Display(Name = "Size Name")]
        public int SizeId { get; set; }

        [ForeignKey("SizeId")]
        [ValidateNever]
        public Size Size { get; set; }

        public Cactus()
        {
            Image = new HashSet<Image>();
            Review = new HashSet<Review>();
        }

        [ValidateNever]
        public virtual ICollection<Image> Image { get; set; }

        [ValidateNever]
        public virtual ICollection<Review> Review { get; set; }

    }
}
