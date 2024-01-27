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
    [Table("Topics")]
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = " khong de trong !")]
        public string Name { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public int? Orders { get; set; }
        [Required(ErrorMessage = " khong de trong !")]
        [AllowHtml]
        public string MetaDescription { get; set; }
        [Required(ErrorMessage = " khong de trong !")]
        public string SeoKeyword { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedBy_At { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Status { get; set; }
    }
}
