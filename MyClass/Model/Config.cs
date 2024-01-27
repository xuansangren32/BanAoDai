using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Configs")]
    public class Config
    {
        [Key]
        public int Id { get; set; }
        public string Site_name { get; set; }
        public string Hot_Line { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
    }
}
