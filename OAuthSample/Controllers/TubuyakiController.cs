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
            switch ((string)Session["service"])
            {
                case "Facebook":
                    return CreateFacebook(message);

                case "Twitter":
                    return CreateTwitter(message);

                default:
                    return RedirectToAction("Index");
            }
        }

        public ActionResult CreateFacebook(string message)
        {
            var accessToken = (string)Session["access_token"];
            var result = Facebook.CreateLoginUsersStatusMessage(accessToken, message);
            TempData["result"] = result;
            return RedirectToAction("Index");
        }

        public ActionResult CreateTwitter(string message)
        {
            var token = (string)Session["accessToken"];
            var tokenSecret = (string)Session["accessSecret"];
            var result = Twitter.UpdateStatus(token, tokenSecret, message);
            TempData["result"] = result;
            return RedirectToAction("Index");
        }
    }
}
