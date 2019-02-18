using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JiYun.NordicTourism.API.Controllers
{
    /// <summary>
    /// Default控制器
    /// </summary>
    [MyAuthorize]   
    public class DefaultController : ApiController
    {        
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult get() {
            return Ok("dddddddddd");
        }
    }
}
