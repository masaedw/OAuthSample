# OAuthSample

This is an OAuth sample program written in C# which explains how to authorize users and to update status with Twitter and Facebook.

* ASP.NET MVC4 (web application framework)
* Hammock (REST library)

## Configuration

### hosts file

OAuth authentication uses HTTP Redirect to switch from the services to your applicaiton.
At this time, some domain name (except 'localhost') is needed.

Edit %SYSTEMROOT%\system32\drivers\etc\hosts file (as Administrator) and add below.

    127.0.0.1       oauth.example.com

Then you can use oauth.example.com to access localhost.

## Twitter

### Developer Registration

   https://dev.twitter.com/

### Application Registration

Create an applicaiton and configure:

* Applicaiton Type: Read and Write

Copy Consumer key and Consumer secret from the application's Details page.

### Web.config

Edit ApplicaitonUrl, TwitterConsumerKey and TwitterConsumerSecret.

    <add key="ApplicationUrl" value="http://oauth.example.com:55108"/>
    <add key="TwitterConsumerKey" value="CONSUMER_KEY"/>
    <add key="TwitterConsumerSecret" value="CONSUMER_SECRET"/>

:55108 is the port number of visual sutdio's debugging web server's. Set the right number or delete it.

## Facebook

### Developer Registration

   https://develpers.facebook.com/

### Applicaiton Registration

Create an application and configure:

*  Web Site: http://oauth.example.com:55108
  :55108 is the port number of visual sutdio's debugging web server's. Set the right number or delete it.

Copy App ID / API Key and Applicaiton Secret.

### Web.config

Edit ApplicaitonUrl, FacebookAppId and FacebookAppSecret.

    <add key="ApplicationUrl" value="http://oauth.example.com:55108"/>
    <add key="FacebookAppId" value="CONSUMER_KEY"/>
    <add key="FacebookAppSecret" value="CONSUMER_SECRET"/>

:55108 is the port of visual sutdio's debugging web server's port. Set the right number or delete it.
