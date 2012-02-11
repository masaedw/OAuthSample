using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;

namespace OAuthSample.Models
{
    public class OAuth
    {
        public static WebConsumer MakeConsumer(string consumerKey, string consumerSecret, string requestTokenURL, string accessTokenURL, string authenticateUrl)
        {
            var description = new ServiceProviderDescription
            {
                RequestTokenEndpoint = new MessageReceivingEndpoint("http://twitter.com/oauth/request_token", HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                UserAuthorizationEndpoint = new MessageReceivingEndpoint("http://twitter.com/oauth/authorize", HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                AccessTokenEndpoint = new MessageReceivingEndpoint("http://twitter.com/oauth/access_token", HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
            };
            //var tokenManager = new InMemoryTokenManager(consumerKey, consumerSecret);
            return new WebConsumer(description, manager);
        }

        class TokenManager : ITokenManager
        {

        }
    }
}