using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using OAuthSample.Models;

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
            var user = (User)Session["user"];
            TempData["result"] = user.Tubuyaki(message);
            return RedirectToAction("Index");
        }
    }
}
