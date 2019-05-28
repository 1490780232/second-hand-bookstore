using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Book
    {
        public string BookId { get; set; }
        public string BookName { get; set; }
        public string BookIbsn { get; set; }
        public string Author { get; set; }
        public int OriPrice { get; set; }
        public string Press { get; set; }
        public int CurrPrice { get; set; }
        public string category { get; set; }

    }
}
