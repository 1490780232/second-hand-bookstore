using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Order
    {
        public string OrderId { get; set; }
        public string BookId { get; set; }
        public string UserId { get; set; }
        public DateTime? OrderTime { get; set; }
        public int? OrderPrice { get; set; }
        public int? OrderStatus { get; set; }
        public string bookName { get; set; }
        public string buyerName { get; set; }
        public string salerName { get; set; }
        public string salerID { get; set; }
    }
}
