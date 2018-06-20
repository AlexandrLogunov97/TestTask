using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace L3.Models
{
    public class CarsFilterViewModel:ICloneable
    {
        public SelectList Brands { get; set; }
        public SelectList Makers { get; set; }
        public SelectList SortList { get; set; }

        public int? MakerId { get; set; }
        public int? BrandId { get; set; }
        public string Sort { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public List<Car> GetFilterResult()
        {
            CarsstoreContext db=new CarsstoreContext();
            var cars = db.Cars.Include(c => c.Brand).Include(c => c.Maker);

            if (MakerId != null && MakerId != 0)
            {
                cars = cars.Where(x => x.Maker.Id == MakerId);
            }
            else
            {
                MakerId = 0;
            }
            if (BrandId != null && BrandId != 0)
            {
                cars = cars.Where(x => x.Brand.Id == BrandId);
            }
            else
            {
                BrandId = 0;
            }
            if (Sort != null && !string.IsNullOrEmpty(Sort) && !Sort.Equals("Default"))
            {
                switch (Sort)
                {
                    case "Ascending":
                    {
                        cars = cars.OrderBy(x => x.Price);
                        break;
                    }
                    case "Descending":
                    {
                        cars = cars.OrderByDescending(x => x.Price);
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }

            List<Maker> makers = db.Makers.ToList();
            List<Brand> brands = db.Brands.ToList();

            List<string> sortList=new List<string>(){"Default", "Ascending", "Descending" };

            makers.Insert(0, new Maker() { Id = 0, Name = "All" });
            brands.Insert(0, new Brand() { Id = 0, Name = "All" });

            Makers = new SelectList(makers, "Id", "Name");
            Brands = new SelectList(brands, "Id", "Name");
            SortList=new SelectList(sortList);
            return cars.ToList();
        }
    }
}