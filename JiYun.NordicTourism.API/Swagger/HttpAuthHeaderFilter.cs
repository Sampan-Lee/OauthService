using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace JiYun.NordicTourism.API.Swagger
{
    public class HttpAuthHeaderFilter: IOperationFilter
    {
        /// <summary>
        /// 应用
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();
            var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline(); //判断是否添加权限过滤器
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Instance).Any(filter => filter is MyAuthorize); //判断是否允许匿名方法 
            var allowAnonymous = apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
            if (isAuthorized && !allowAnonymous)
            {
                operation.parameters.Add(new Parameter { name = "Authorization", @in = "header", description = "安全", required = false, type = "string" });
            }
        }
    }
}