using System;
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
        public static string ApplicationUrl { get; set; }

        static Config()
        {
            TwitterConsumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"];
            TwitterConsumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
            ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
        }
    }
}