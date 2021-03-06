﻿using JiYun.AuthorizationService.Repositories.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JiYun.AuthorizationService.Controllers
{
    public class HomeController : Controller
    {
        public IAuthorizationInfoRepository authorizationInfoRepository { get; set; }
        public ActionResult Index()
        {
            return Content("Hello World");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}