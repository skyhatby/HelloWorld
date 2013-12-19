using System.Data;
using System.Linq;
using System.Web.Mvc;
using HelloWorld.Filters;
using HelloWorld.Models;
using WebMatrix.WebData;

namespace HelloWorld.Controllers
{
    [Culture]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private HelloWorldDb db = new HelloWorldDb();


        //
        // GET: /Account/Manage

        public ActionResult DropPassword(int userId)
        {
            var user = db.UserProfiles.Find(userId);
            var token = WebSecurity.GeneratePasswordResetToken(user.UserName);
            WebSecurity.ResetPassword(token, "111111");
            return View();
        }


        //
        // GET: /User/

        public ActionResult Index()
        {
            return View(db.UserProfiles.ToList());
        }


        
        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userprofile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userprofile);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            db.UserProfiles.Remove(userprofile);
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