using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace L3.Models
{
    public class CarsstoreContext:DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Maker> Makers { get; set; }
        public DbSet<Brand> Brands { get; set; }
    }
}