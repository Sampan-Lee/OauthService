using System.Web.Http;
using WebActivatorEx;
using JiYun.NordicTourism.API;
using Swashbuckle.Application;
using JiYun.NordicTourism.API.Swagger;
using System.Linq;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace JiYun.NordicTourism.API
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "JiYun.NordicTourism.APIÎÄµµ");
                        c.IncludeXmlComments($"{System.AppDomain.CurrentDomain.BaseDirectory}/bin/JiYun.NordicTourism.API.xml");
                        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                        c.CustomProvider((defaultProvider) => new CachingSwaggerProvider(defaultProvider));
                        c.OperationFilter<HttpAuthHeaderFilter>();
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.EnableApiKeySupport("Authorization", "header");
                        c.DocumentTitle("SwaggerTest_API");
                        c.InjectJavaScript(thisAssembly, "JiYun.NordicTourism.API.Swagger.Swagger_lang_zh.js");
                    });
        }
    }
}
