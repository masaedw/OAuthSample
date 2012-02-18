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
    public class AccountLoginModel
    {
        public string Service { get; set; }
    }

    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View(new AccountLoginModel());
        }

        [HttpPost]
        public ActionResult Login(string service)
        {
            var credentials = new OAuthCredentials
            {
                Type = OAuthType.RequestToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = Config.TwitterConsumerKey,
                ConsumerSecret = Config.TwitterConsumerSecret,
                CallbackUrl = Config.ApplicationUrl + "/Account/Callback",
            };

            var client = new RestClient
            {
                Authority = "https://twitter.com/oauth",
                Credentials = credentials
            };

            var request = new RestRequest
            {
                Path = "/request_token"
            };

            var response = client.Request(request);
            var collection = HttpUtility.ParseQueryString(response.Content);
            Session["requestSecret"] = collection[1];
            return Redirect("https://twitter.com/oauth/authenticate?oauth_token=" + collection[0]);
        }

        public ActionResult Callback(string oauth_token, string oauth_verifier)
        {
            var requestSecret = (string)Session["requestSecret"];

            var credentials = new OAuthCredentials
            {
                Type = OAuthType.AccessToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = Config.TwitterConsumerKey,
                ConsumerSecret = Config.TwitterConsumerSecret,
                Token = oauth_token,
                TokenSecret = requestSecret,
                Verifier = oauth_verifier,
            };

            var client = new RestClient()
            {
                Authority = "https://twitter.com/oauth",
                Credentials = credentials,
            };

            var request = new RestRequest
            {
                Path = "/access_token"
            };

            var response = client.Request(request);
            var collection = HttpUtility.ParseQueryString(response.Content);

            Session["accessToken"] = collection["oauth_token"];
            Session["accessSecret"] = collection["oauth_token_secret"];

            return RedirectToAction("Index", "Tubuyaki");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("login");
        }
    }
}
