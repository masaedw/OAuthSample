﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OAuthSample.Models
{
    public class Config
    {
        public static string TwitterConsumerKey { get; set; }
        public static string TwitterConsumerSecret { get; set; }
        public static string FacebookAppId { get; set; }
        public static string FacebookAppSecret { get; set; }
        public static string MixiConsumerKey { get; set; }
        public static string MixiConsumerSecret { get; set; }
        public static string ApplicationUrl { get; set; }

        static Config()
        {
            TwitterConsumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"];
            TwitterConsumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
            FacebookAppId = ConfigurationManager.AppSettings["FacebookAppId"];
            FacebookAppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"];
            MixiConsumerKey = ConfigurationManager.AppSettings["MixiConsumerKey"];
            MixiConsumerSecret = ConfigurationManager.AppSettings["MixiConsumerSecret"];
            ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
        }
    }
}