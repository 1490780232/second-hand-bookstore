using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class CheckBook
    {
        public string BookId { get; set; }
        public int? CheckStatus { get; set; }
        public string FaileReason { get; set; }
        public string AdminId { get; set; }
        public DateTime? CheckTime { get; set; }
    }
}
