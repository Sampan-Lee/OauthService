using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace JiYun.AuthorizationServer.App_Start
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            // MVC - OPTIONAL: Enable property injection into action filters.
            //builder.RegisterFilterProvider();
            //builder.RegisterControllers(typeof(WebApiApplication).Assembly).PropertiesAutowired();
            builder.RegisterTypes(Assembly.GetExecutingAssembly().GetTypes()).PropertiesAutowired().AsImplementedInterfaces();
            builder.RegisterTypes(Assembly.GetExecutingAssembly().GetTypes()).PropertiesAutowired();
            //获取所有程序集
            //var assemblies = System.Web.Compilation.BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            //var baseType = typeof(IDependency);
            ////自动注册接口
            //builder.RegisterAssemblyTypes(assemblies).Where(b => b.GetInterfaces().
            //Any(c => c == baseType && b != baseType)).AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired().UsingConstructor();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}