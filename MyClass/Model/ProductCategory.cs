using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("ProductCategorys")]
    internal class ProductCategory
    {
       
        [Key]
       
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        [Required]
        [StringLength(150)]
        public string Alias { get; set; }
        public string Description { get; set; }
        [StringLength(250)]
        public string Icon { get; set; }
        [StringLength(250)]

        public string SeoTitle { get; set; }
        [StringLength(550)]
        public string SeoDescription { get; set; }
        [StringLength(250)]
        public string SeoKeyword { get; set; }
        
    }
}
