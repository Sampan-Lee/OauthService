using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace JiYun.AuthorizationService
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterTypes(Assembly.GetExecutingAssembly().GetTypes()).PropertiesAutowired().AsImplementedInterfaces();
            builder.RegisterTypes(Assembly.GetExecutingAssembly().GetTypes()).PropertiesAutowired();            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}