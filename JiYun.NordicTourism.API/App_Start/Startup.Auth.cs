using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;

namespace JiYun.NordicTourism.API
{
	public partial class Startup
	{
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
}