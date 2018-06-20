using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace L3.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Price { get; set; }
        public int Power { get; set; }
        public string Equipment { get; set; }
        public int Year { get; set; }
        public int EngineCapacity { get; set; }
        public string About { get; set; }
        public byte[] Image { get; set; }

        #region FK_cars
        [Required]
        public int? MakerId { get; set; }
        public Maker Maker { get; set; }
        [Required]
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }
#endregion
    }
}