using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public partial class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string AdminId { get; set; }
        public string AdminName { get; set; }
        public string PhoneNum { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}
