﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuthSample.Models
{
    public class TubuyakiResult
    {
        public dynamic TwitterResult { get; set; }
        public dynamic FacebookResult { get; set; }
    }

    public class User
    {
        public string TwitterAccessToken { get; set; }
        public string TwitterAccessTokenSecret { get; set; }
        public dynamic TwitterUser { get; set; }

        public string FacebookAccessToken { get; set; }
        public dynamic FacebookUser { get; set; }

        public TubuyakiResult Tubuyaki(string message)
        {
            return new TubuyakiResult
            {
                TwitterResult = Twitter.UpdateStatus(TwitterAccessToken, TwitterAccessTokenSecret, message),
                FacebookResult = Facebook.CreateLoginUsersStatusMessage(FacebookAccessToken, message),
            };
        }
    }
}