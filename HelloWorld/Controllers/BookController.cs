using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
            ViewBag.BookId = id;
            return View(db.Chapters.Where(r => r.BookId==id).ToList());
        }

        //
        // GET: /Chapter/Create

        public ActionResult ChapterCreate(int id)
        {
            ViewBag.BookId = id;
            return View();
        }

        //
        // POST: /Chapter/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChapterCreate(Chapter chapter,int id)
        {
            if (ModelState.IsValid)
            {
                db.Chapters.Add(chapter);
                chapter.BookId = id;
                db.SaveChanges();
                return RedirectToAction("Chapters",new {id});
            }

            return View(chapter);
        }

        //
        // GET: /Chapter/Edit/5

        public ActionResult ChapterEdit(int id)
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
        public ActionResult ChapterEdit(Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chapter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Chapters",new {id = chapter.BookId});
            }
            return View(chapter);
        }

        //
        // GET: /Chapter/Delete/5

        public ActionResult ChapterDelete(int id)
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

        [HttpPost, ActionName("ChapterDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ChapterDeleteConfirmed(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            db.Chapters.Remove(chapter);
            db.SaveChanges();
            return RedirectToAction("Chapters",new {id = chapter.BookId});
        }

        //
        // GET: /Chapter/Details/5

        public ActionResult ChapterDetails(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
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