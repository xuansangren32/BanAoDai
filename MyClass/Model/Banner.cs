using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Banners")]
    public class Banner
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Khong de trong!")]
        public String Name { get; set; }
        public String Link { get; set; }
        public String Img { get; set; }
        [Required(ErrorMessage = "Khong de trong!")]
        public string Position { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedBy_At { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Status { get; set; }

    }
}
