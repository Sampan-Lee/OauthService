using Autofac;
using Autofac.Integration.Mvc;
using JiYun.AuthorizationServer.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace JiYun.AuthorizationServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            AutofacConfig.Register();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }        
    }
}
