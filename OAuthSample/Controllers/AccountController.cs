using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OAuth;

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
            var description = new ServiceProviderDescription
            {
                RequestTokenEndpoint
            };
            var consumer = new WebConsumer();
            return View(new AccountLoginModel { Service = service });
        }
    }
}
