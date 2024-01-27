using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyClass.Model
{
    [Table("Categorys")]
    public class Category
    {
        //public Category()
        //{
        //    this.Products = new HashSet<Product>();
        //}
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên danh mục không để trống")]
        [StringLength(250)]
        public string Name { get; set; }

        public String Slug { get; set; }
      
        public int? ParentId { get; set; }
        public int? Orders { get; set; }
        [Required(ErrorMessage ="Mục này không để trống !")]
        [AllowHtml]
        public string MetaDescription { get; set; }
        [Required(ErrorMessage = "Mô tả chi tiết không được để trống !")]
        public string SeoKeyword { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedBy_At { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Status { get; set; }
        //public ICollection<Product> Products { get; set; }

    }
}
