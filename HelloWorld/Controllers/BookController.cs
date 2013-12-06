using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using HelloWorld.Filters;
using HelloWorld.Models;

namespace HelloWorld.Controllers
{
    [Culture]
    [Authorize]
    public class BookController : Controller
    {
        private HelloWorldDb db = new HelloWorldDb();

        //
        // GET: /Book/

        public ActionResult Index()
        {
            var books = db.Books.Where(d => d.UserProfile.UserName == User.Identity.Name);
            return View(books.ToList());
        }

        //[Authorize(Roles = "Admin")]
        public ActionResult IndexAll()
        {
            var books = db.Books.Include(m => m.UserProfile);
            return View(books.ToList());
        }

        //
        // GET: /Book/Details/5

        public ActionResult Chapters(int id)
        {
            return View(db.Chapters.Where(r => r.BookId==id).ToList());
        }

        //
        // GET: /Book/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            return View();
        }

        //
        // POST: /Book/Create

        private int FindColomnByUserName()
        {
            return (from userProfile in db.UserProfiles where userProfile.UserName == User.Identity.Name select userProfile.UserId).First();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                book.UserId = FindColomnByUserName();
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", book.UserId);
            return View(book);
        }

        //
        // GET: /Book/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Book book = db.Books.Find(id);
            if (book.UserProfile.UserName == User.Identity.Name)
            {
                if (book == null)
                {
                    return HttpNotFound();
                }
                ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", book.UserId);
                return View(book);
            }
            return null;
        }

        //
        // POST: /Book/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", book.UserId);
            return View(book);
        }

        //
        // GET: /Book/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Book book = db.Books.Find(id);
            if (book.UserProfile.UserName == User.Identity.Name)
            {
                if (book == null)
                {
                    return HttpNotFound();
                }
                return View(book);
            }
            return null;
        }

        //
        // POST: /Book/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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