using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class SpecialType
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Special Tag Type")]
        public string SpecialTagType { get; set; }
    }
}
