using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImportWeb.Repositories;

namespace ImportWeb.Controllers
{
    public class SavedDataController : Controller
    {
        // GET: SavedData
        public ActionResult Index()
        {
            var pr = new ProcessedDataRepository();
            var model = pr.GetAllData();
            return View(model);
        }
    }
}