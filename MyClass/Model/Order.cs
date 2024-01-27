using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Orders")]
    public class Order
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        [Required(ErrorMessage = "Tên khách hàng không được để trống!")]
        public string ReceiveName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        public string ReceivePhone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        public string ReceiveAddress { get; set; }
        public string ReceiveEmail { get; set; }
        public string Note { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public int TypePayment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedBy_At { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Status { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
