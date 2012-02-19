using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hammock;
using Codeplex.Data;

namespace OAuthSample.Models
{
    public class Facebook
    {
        /// <summary>
        /// ログインユーザ情報を取得します
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static dynamic GetUserInformation(string accessToken)
        {
            // see http://developers.facebook.com/docs/reference/api/user/

            var client = new RestClient { Authority = "https://graph.facebook.com/" };
            var request = new RestRequest { Path = "me" };
            request.AddParameter("access_token", accessToken);
            var response = client.Request(request);
            return DynamicJson.Parse(response.Content);
        }

        /// <summary>
        /// ログインユーザのステータスメッセージを更新します
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static dynamic CreateLoginUsersStatusMessage(string accessToken, string message)
        {
            // see http://developers.facebook.com/docs/reference/api/user/

            var client = new RestClient { Authority = "https://graph.facebook.com/" };
            var request = new RestRequest
            {
                Path = "me/feed",
                Method = Hammock.Web.WebMethod.Post,
            };
            request.AddParameter("access_token", accessToken);
            request.AddParameter("message", message);
            var response = client.Request(request);
            return DynamicJson.Parse(response.Content);
        }
    }
}