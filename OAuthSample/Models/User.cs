using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuthSample.Models
{
    public class User
    {
        public string TwitterAccessToken { get; set; }
        public string TwitterAccessTokenSecret { get; set; }
        public dynamic TwitterUser { get; set; }

        public string FacebookAccessToken { get; set; }
        public dynamic FacebookUser { get; set; }
    }
}