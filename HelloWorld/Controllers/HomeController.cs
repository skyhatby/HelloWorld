using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloWorld.Filters;
using HelloWorld.Models;

namespace HelloWorld.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        HelloWorldDb db = new HelloWorldDb();

        public ActionResult Index()
        {
            var model = db.UserProfiles.ToList();
            return View(model);
        }

        public ActionResult ChangeCulture(string lang)
        {
            var returnUrl = Request.UrlReferrer.AbsolutePath;
            // Список культур
            var cultures = new List<string> { "ru", "en" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;   // если куки уже установлено, то обновляем значение
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

        public ActionResult About()
        {
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
