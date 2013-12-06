using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarkdownDeepSample.Controllers
{
	[HandleError]
	public class MarkdownDeepController : Controller
	{
		static string m_Content=
@"# Edit this Page with MarkdownDeep

This demo project shows how to use MarkdownDeep in a typical ASP.NET MVC application.

* Click the *Edit this Page* link below to make changes to this page with MarkdownDeep's editor
* Use the links in the top right for more info.
* Look at the file `MarkdownDeepController.cs` for implementation details.

MarkdownDeep is written by [Topten Software](http://www.toptensoftware.com).  The project home for MarkdownDeep is [here](http://www.toptensoftware.com/markdowndeep).

";

		public ActionResult Index()
		{
			var md = new MarkdownDeep.Markdown();
			md.SafeMode = true;
			md.ExtraMode = true;

			ViewData["Content"] = md.Transform(m_Content);
			return View();
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Edit()
		{
			// For editing the content, just pass the raw Markdown to the view
			ViewData["content"] = m_Content;
			return View();
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Edit(string content)
		{
			// Save the content and switch back to the main view
			m_Content = content;
			return RedirectToAction("Index");
		}

		
		
	}
}
