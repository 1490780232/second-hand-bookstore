﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class OrderStatistics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string time { get; set; }
        public int BookNum { get; set; }
        public int TotalValue { get; set; }
    }
}
