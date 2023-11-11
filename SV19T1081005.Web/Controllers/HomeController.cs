using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV19T1081005.Web.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
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
       
        public ActionResult Categories(int page = 1, string searchValue = "")
        {
            ViewBag.Title = "Categories";
            int pageSize = 10;
            int rowCount = 0;

            var data = BusinessLayer.CommonDataService.ListOfCategories(page,
                                                                       pageSize,
                                                                       searchValue,
                                                                       out rowCount);
            Models.CategoryPaginationResult model = new Models.CategoryPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = data
            };
            return View(model);
        }
    }
}