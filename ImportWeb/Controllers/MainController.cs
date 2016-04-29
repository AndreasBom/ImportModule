using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImportWeb.Models;

namespace ImportWeb.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            var m = new MainViewModel();
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Index(FormCollection collection)
        {
            var model = new MainViewModel();
            if (ModelState.IsValid)
            {
                UpdateModel(model);
            }
            
            return RedirectToAction("Index");
        }
    }
}