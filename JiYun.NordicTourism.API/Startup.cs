using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(JiYun.NordicTourism.API.Startup))]

namespace JiYun.NordicTourism.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}