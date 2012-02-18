using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;

namespace OAuthSample.Controllers
{
    public class TubuyakiController : Controller
    {
        public static string TwitterConsumerKey { get; set; }
        public static string TwitterConsumerSecret { get; set; }
        public static string ApplicationUrl { get; set; }

        static TubuyakiController()
        {
            TwitterConsumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"];
            TwitterConsumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
            ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
        }

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
                TwitterConsumerKey,
                TwitterConsumerSecret,
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
