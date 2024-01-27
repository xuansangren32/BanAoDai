using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên danh mục không để trống")]
        [StringLength(250)]
        public String Name { get; set; }
     
        public String Link { get; set; }
        public String Slug { get; set; }
        public int? TableId { get; set; }
        public String TypeMenu { get; set; }
        public String Position { get; set; }
        public int? ParentId { get; set; }
        public int? Orders { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedBy_At { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Status { get; set; }
    }
}
