using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class Products
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Image { get; set; }
        [Display(Name = "Product Colour")]
        public string ProductColor { get; set; }

        [Required]
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }
        [Display(Name = "Product Type")]
        [Required]
        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]

        public ProductType ProductType { get; set; }
        [Display(Name = "Special Type")]
        [Required]
        public int SpecialTypeId { get; set; }
        [ForeignKey("SpecialTypeId")]
        public SpecialType SpecialType { get; set; }
    }
}
