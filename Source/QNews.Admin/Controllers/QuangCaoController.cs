using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QNews.Base;

namespace QNews.Web.Controllers
{
    public class QuangCaoController : Controller
    {
        private QNewsDBContext db = new QNewsDBContext();

        // GET: QuangCao
        public ActionResult Index()
        {
            var advertises = db.Advertises.Include(a => a.AspNetUser_CreateBy).Include(a => a.AspNetUser_ModifyBy).Include(a => a.Status);
            return View(advertises.ToList());
        }

        // GET: QuangCao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertise advertise = db.Advertises.Find(id);
            if (advertise == null)
            {
                return HttpNotFound();
            }
            return View(advertise);
        }

        // GET: QuangCao/Create
        public ActionResult Create()
        {
            ViewBag.CreateBy = new SelectList(db.AspNetUsers, "Id", "UserFullName");
            ViewBag.ModifyBy = new SelectList(db.AspNetUsers, "Id", "UserFullName");
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name");
            return View();
        }

        // POST: QuangCao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,Image,ZoneID,Order,Link,NgayBatDau,NgayKetThuc,CreateDate,CreateBy,ModifyDate,ModifyBy,Viewed,StatusID,IsRemoved")] Advertise advertise)
        {
            if (ModelState.IsValid)
            {
                db.Advertises.Add(advertise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreateBy = new SelectList(db.AspNetUsers, "Id", "UserFullName", advertise.CreateBy);
            ViewBag.ModifyBy = new SelectList(db.AspNetUsers, "Id", "UserFullName", advertise.ModifyBy);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name", advertise.StatusID);
            return View(advertise);
        }

        // GET: QuangCao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertise advertise = db.Advertises.Find(id);
            if (advertise == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = new SelectList(db.AspNetUsers, "Id", "UserFullName", advertise.CreateBy);
            ViewBag.ModifyBy = new SelectList(db.AspNetUsers, "Id", "UserFullName", advertise.ModifyBy);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name", advertise.StatusID);
            return View(advertise);
        }

        // POST: QuangCao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Image,ZoneID,Order,Link,NgayBatDau,NgayKetThuc,CreateDate,CreateBy,ModifyDate,ModifyBy,Viewed,StatusID,IsRemoved")] Advertise advertise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(advertise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = new SelectList(db.AspNetUsers, "Id", "UserFullName", advertise.CreateBy);
            ViewBag.ModifyBy = new SelectList(db.AspNetUsers, "Id", "UserFullName", advertise.ModifyBy);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name", advertise.StatusID);
            return View(advertise);
        }

        // GET: QuangCao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertise advertise = db.Advertises.Find(id);
            if (advertise == null)
            {
                return HttpNotFound();
            }
            return View(advertise);
        }

        // POST: QuangCao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Advertise advertise = db.Advertises.Find(id);
            db.Advertises.Remove(advertise);
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
