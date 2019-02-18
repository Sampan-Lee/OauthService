using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using JiYun.AuthorizationService.Providers;
using Microsoft.Owin.Security.Infrastructure;

namespace JiYun.AuthorizationService
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }   
        public void ConfigureAuth(IAppBuilder app)
        {            
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(10),
                Provider = new JyAuthorizationServerProvider(),
                RefreshTokenProvider = new JyRefreshTokenProvider()
            };
            app.UseOAuthAuthorizationServer(OAuthOptions);            
        }
    }
}