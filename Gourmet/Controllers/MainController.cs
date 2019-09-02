using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Cfg;
using Gourmet.Models;

namespace Gourmet.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Ресторан Gourmet";

            return View();
        }
    }
}