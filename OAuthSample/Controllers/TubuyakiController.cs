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
            var token = (string)Session["accessToken"];
            var tokenSecret = (string)Session["accessSecret"];

            var client = new RestClient
            {
                Authority = "http://api.twitter.com",
                UserAgent = "OAuthSample",
            };

            var credentials = OAuthCredentials.ForProtectedResource(
                Config.TwitterConsumerKey,
                Config.TwitterConsumerSecret,
                token,
                tokenSecret);
            credentials.ParameterHandling = OAuthParameterHandling.UrlOrPostParameters;

            var request = new RestRequest
            {
                Path = "statuses/update.json",
                Method = WebMethod.Post,
                Credentials = credentials,
            };

            request.AddParameter("status", message);

            var response = client.Request(request);
            TempData["result"] = response.Content;

            return RedirectToAction("Index");
        }
    }
}
