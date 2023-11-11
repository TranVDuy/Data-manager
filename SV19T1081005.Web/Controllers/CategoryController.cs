using SV19T1081005.BusinessLayer;
using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV19T1081005.Web.Controllers
{
    [Authorize]
    [RoutePrefix("category")]
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            //Lấy điều kiện tìm kiếm từ trong session
            Models.PaginationSearchInput model = Session["CATEGORY_SEARCH"] as Models.PaginationSearchInput;

            //Nếu không có điều kiện tìm kiếm thì tạo điều kiện mặc định
            if (model == null)
                model = new Models.PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = ""
                };

            return View(model);
        }
        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;

            var data = BusinessLayer.CommonDataService.ListOfCategories(input.Page,
                                                                       input.PageSize,
                                                                       input.SearchValue,
                                                                       out rowCount);
            Models.CategoryPaginationResult model = new Models.CategoryPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm vào session
            Session["CATEGORY_SEARCH"] = input;

            return View(model);
        }
        /// <summary>
        /// Tìm kiếm từ mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        [Route("searchfromproduct/{categoryID}")]
        public ActionResult SearchFromProduct(string method, string categoryID)
        {

            Category sp = CommonDataService.GetCategory(Convert.ToInt32(categoryID));
            Session["CATEGORY_SEARCH"] = new Models.PaginationSearchInput()
            {
                Page = 1,
                PageSize = 10,
                SearchValue = sp.CategoryName
            };

            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Category model = new Category()
            {
                CategoryID = 0
            };
            ViewBag.Title = "Thêm loại hàng";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Save(Category model)
        {
            //Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(model.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Tên loại hàng không được để trống!");
            }
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                model.Description = "";
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CategoryID == 0 ? "Nhập lại thông tin loại hàng" : "-----";
                return View("Create", model);
            }

            if (model.CategoryID > 0)
            {
                CommonDataService.UpdateCategory(model);
            }
            else
            {
                CommonDataService.AddCategory(model);
            }

            Session["CATEGORY_SEARCH"] = new Models.PaginationSearchInput()
            {
                Page = 1,
                PageSize = 10,
                SearchValue = model.CategoryName
            };

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        /// 
        [Route("edit/{categoryID}")]
        public ActionResult Edit(int categoryID)
        {
            Category model = BusinessLayer.CommonDataService.GetCategory(categoryID);
            if(model == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Cập nhật thông tin loại hàng";
            return View("Create", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        /// 
        [Route("delete/{categoryID}")]
        public ActionResult Delete(int categoryID)
        {
            if(Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCategory(categoryID);
                return RedirectToAction("Index");
            }

            Category model = CommonDataService.GetCategory(categoryID);
            if(model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

    }
}