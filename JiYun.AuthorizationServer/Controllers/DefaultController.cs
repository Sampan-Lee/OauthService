using JiYun.AuthorizationServer.Repositories.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JiYun.AuthorizationServer.Controllers
{
    public class DefaultController : Controller
    {
        public IAuthorizationInfoRepository authorizationInfoRepository { get; set; }
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
    }
}