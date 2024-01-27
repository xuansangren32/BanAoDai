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
    [Table("Posts")]
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public int TopicId { get; set; }
        [Required(ErrorMessage = "Mo ta SEO khong de trong !")]
        public String Title { get; set; }
        public string Slug { get; set; }
        [AllowHtml]
        public string Detail { get; set; }
        public String Img { get; set; }
        public string PostType { get; set; }
        //[Required(ErrorMessage = "Mục này không để trống !")]
        //[AllowHtml]
        public string MetaDescription { get; set; }
       
        public string SeoKeyword { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedBy_At { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Status { get; set; }
    }
}
