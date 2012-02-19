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
            switch (service)
            {
                case "Twitter":
                    return LoginWithTwitter();

                case "Facebook":
                    return LoginWithFacebook();

                default:
                    return RedirectToAction("Index");
            }
        }

        public ActionResult LoginWithTwitter()
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

        public ActionResult LoginWithFacebook()
        {
            var clientId = Config.FacebookAppId;
            var callback = HttpUtility.UrlEncode(Config.ApplicationUrl + "/Account/CallbackFacebook");

            // offline_access はアクセストークンを永続化したい場合に必要となる
            // これがない場合は使い切りのトークンしか貰えない
            var requestUrl = String.Format("https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope=offline_access,publish_stream",
                clientId,
                callback);
            return Redirect(requestUrl);
        }

        public ActionResult CallbackFacebook(string code)
        {
            if (String.IsNullOrEmpty(code))
            {
                TempData["message"] = "認証に失敗しました";
                return RedirectToAction("Login");
            }
            TempData["message"] = "認証に成功しました" + code;
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("login");
        }
    }
}
