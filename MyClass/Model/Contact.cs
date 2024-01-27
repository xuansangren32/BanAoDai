using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Contacts")]
    public class Contact
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 ký tự")]
        public string Name { get; set; }
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 ký tự")]
        public string Email { get; set; }
        public string Wedsite { get; set; }
        [StringLength(4000)]
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
