using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloWorld.Models;

namespace HelloWorld.Controllers
{
    public class ChapterController : Controller
    {
        private HelloWorldDb db = new HelloWorldDb();

        //
        // GET: /Chapter/

        public ActionResult Index()
        {
            return View(db.Chapters.ToList());
        }

        //
        // GET: /Chapter/Details/5

        public ActionResult Details(int id = 0)
        {
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        //
        // GET: /Chapter/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Chapter/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Chapters.Add(chapter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chapter);
        }

        //
        // GET: /Chapter/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        //
        // POST: /Chapter/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chapter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chapter);
        }

        //
        // GET: /Chapter/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        //
        // POST: /Chapter/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            db.Chapters.Remove(chapter);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}