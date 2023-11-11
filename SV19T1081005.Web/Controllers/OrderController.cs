using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV19T1081005.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        // GET: Order

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}