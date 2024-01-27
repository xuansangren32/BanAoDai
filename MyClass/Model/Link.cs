using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{

    [Table("Links")]
    public class Link
    {

        [Key]
        
        public int ID { get; set; }
        public String Slug { get; set; }
        public string TypeLink { get; set; }
        public int TableId { set; get; }
       
    }
}
