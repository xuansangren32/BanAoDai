using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("ProductSales")]
    public class ProductSale
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Product_id { get; set; }
        public decimal? PriceSale { get; set; }
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set;}
    }
}
