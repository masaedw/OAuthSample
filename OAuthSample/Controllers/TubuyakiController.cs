using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAuthSample.Controllers
{
    public class TubuyakiController : Controller
    {
        //
        // GET: /Tubuyaki/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string message)
        {
            return RedirectToAction("Index");
        }
    }
}
