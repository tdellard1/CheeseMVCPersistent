using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddCategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Category { get; set; }
    }
}
