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
            // To delete this dispatch method, see this blog post:
            // http://www.dotnetcurry.com/ShowArticle.aspx?ID=724
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

        #region Twitter

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

        #endregion

        #region Facebook

        // This is an implementation of facebook authentication.
        // The protocol documented in
        // https://developers.facebook.com/docs/authentication/

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
                // TempData is equivalent to flash of rails.
                TempData["message"] = "認証に失敗しました";
                return RedirectToAction("Login");
            }

            var client = new RestClient
            {
                Authority = "https://graph.facebook.com/oauth/",
            };

            var request = new RestRequest
            {
                Path = "access_token",
            };

            request.AddParameter("client_id", Config.FacebookAppId);
            request.AddParameter("redirect_uri", Config.ApplicationUrl + "/Account/CallbackFacebook");
            request.AddParameter("client_secret", Config.FacebookAppSecret);
            request.AddParameter("code", code);

            var response = client.Request(request);

            // response contains access_token and expires
            var result = HttpUtility.ParseQueryString(response.Content);

            // to handle expired access tokens, see the blog
            // https://developers.facebook.com/blog/post/500/

            var accessToken = result["access_token"];
            Session["access_token"] = accessToken;
            Session["expires"] = result["expires"];
            Session["user"] = Facebook.GetUserInformation(accessToken);

            return RedirectToAction("Index", "Tubuyaki");
        }

        #endregion

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("login");
        }
    }
}
