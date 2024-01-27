using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanAoDai.Models
{
    public class CustomerViewModel
    {
        [Required(ErrorMessage = "Tên khách hàng không được để trống!")]
        public string ReceiveName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        public string ReceivePhone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        public string ReceiveAddress { get; set; }
        public string ReceiveEmail { get; set; }
        public string Payment { get; set; }
    }
}