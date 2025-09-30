using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Models
{
    public class RecentOrder
    {
        public int OrderID { get; set; }          // ID đơn hàng
        public decimal TotalAmount { get; set; }  // Tổng tiền
        public string Status { get; set; }        // Trạng thái đơn hàng
        public DateTime OrderDate { get; set; }   // Ngày đặt hàng
    }
}

