using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class BookStatu
    {
        public string BookId { get; set; }
        public string BookcaseId { get; set; }
        public int? BookStatus { get; set; }
        public int? CheckStatus { get; set; }
        //public string Rfid { get; set; }
        //public string SellerId { get; set; }
        public DateTime? STime { get; set; }
    }
}
