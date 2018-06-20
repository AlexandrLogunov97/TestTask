using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace L3.Models
{
    public class Maker
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string About { get; set; }
    }
}