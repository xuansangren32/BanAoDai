using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyClass.Model
{
    [Table("Products")]
    public class Product
    {
        public Product()
        {
            this.ProductImages = new HashSet<ProductImage>();
            this.OrderDetails = new HashSet<OrderDetail>();
           
           
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CatId { get; set; }
      
        public int BrandId { get; set; }
        [Required(ErrorMessage ="Ten san pham khong de trong !")]

        public string Name { get; set; }
        public String Slug { get; set; }

        public int Qty { get; set; }
        public decimal PriceBuy { get; set; }
        public decimal? OriginaPrice { get; set; }
        public decimal? PriceSale { get; set; }
        public bool IsHome { get; set; }
        public bool IsActive { get; set; }

        [AllowHtml]
        public string Detail { get; set; }
        [Required(ErrorMessage = "Mục này không để trống !")]
        [AllowHtml]
        public string MetaDescription { get; set; }
        [Required(ErrorMessage = "Tu Khoa SEO khong de trong !")]
        public string SeoKeyword { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedBy_At { get; set; }
        public int? UpdatedBy { get; set; }
       
        public DateTime? UpdatedAt { get; set; }
        public int? Status { get; set; }
       
        public virtual ICollection<ProductImage> ProductImages { get; set; }
       
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
       
        public string Img { get; set; }
    }
}
