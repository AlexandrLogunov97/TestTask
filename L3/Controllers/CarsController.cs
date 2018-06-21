using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using L3.Filters;
using L3.Models;
using PagedList;

namespace L3.Controllers
{
    public class CarsController : Controller
    {
        private CarsstoreContext db = new CarsstoreContext();

        // GET: Cars
        public ActionResult Index(int? page,CarsFilterViewModel filter)
        {
            var cars = filter.GetFilterResult();
            ViewBag.Brands = filter.Brands;
            ViewBag.Makers = filter.Makers;
            ViewBag.Maker = filter.MakerId;
            ViewBag.Brand = filter.BrandId;
            ViewBag.Sort = filter.Sort;
            ViewBag.SortList = filter.SortList;
            ViewBag.Filter = (CarsFilterViewModel)filter.Clone();

            return View(cars.ToList().ToPagedList(page ?? 1, 4));
        }

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cars = db.Cars.Include(x => x.Brand).Include(x => x.Maker);
            Car car = cars.ToList().Find(x=>x.Id==id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        [Auth]
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name");
            ViewBag.MakerId = new SelectList(db.Makers, "Id", "Name");
            return View();
        }

        // POST: Cars/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Power,Equipment,Year,EngineCapacity,About,MakerId,BrandId")] Car car, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                if (uploadImage != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }

                    car.Image = imageData;
                }
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", car.BrandId);
            ViewBag.MakerId = new SelectList(db.Makers, "Id", "Name", car.MakerId);
            return View(car);
        }

        // GET: Cars/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", car.BrandId);
            ViewBag.MakerId = new SelectList(db.Makers, "Id", "Name", car.MakerId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Power,Equipment,Year,EngineCapacity,About,MakerId,BrandId")] Car car,HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                if (uploadImage != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }

                    car.Image = imageData;
                }

                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", car.BrandId);
            ViewBag.MakerId = new SelectList(db.Makers, "Id", "Name", car.MakerId);
            return View(car);
        }

        // GET: Cars/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
