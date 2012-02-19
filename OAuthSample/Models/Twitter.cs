using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Codeplex.Data;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;

namespace OAuthSample.Models
{
    public class Twitter
    {
        /// <summary>
        /// ステータスを更新します
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="accessTokenSecret"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static dynamic UpdateStatus(string accessToken, string accessTokenSecret, string message)
        {
            var client = new RestClient
            {
                Authority = "http://api.twitter.com",
                UserAgent = "OAuthSample",
            };

            var credentials = OAuthCredentials.ForProtectedResource(
                Config.TwitterConsumerKey,
                Config.TwitterConsumerSecret,
                accessToken,
                accessTokenSecret);
            credentials.ParameterHandling = OAuthParameterHandling.UrlOrPostParameters;

            var request = new RestRequest
            {
                Path = "statuses/update.json",
                Method = WebMethod.Post,
                Credentials = credentials,
            };

            request.AddParameter("status", message);

            var response = client.Request(request);
            return DynamicJson.Parse(response.Content);
        }
    }
}