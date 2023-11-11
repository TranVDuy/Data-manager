using SV19T1081005.BusinessLayer;
using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV19T1081005.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("shipper")]
    public class ShipperController : Controller
    {
        // GET: Shipper
        public ActionResult Index()
        {
            //Lấy điều kiện tìm kiếm từ trong session
            Models.PaginationSearchInput model = Session["SHIPPER_SEARCH"] as Models.PaginationSearchInput;

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

            var data = BusinessLayer.CommonDataService.ListOfShippers(input.Page,
                                                                       input.PageSize,
                                                                       input.SearchValue,
                                                                       out rowCount);
            Models.ShipperPaginationResult model = new Models.ShipperPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm vào session
            Session["SHIPPER_SEARCH"] = input;

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Shipper model = new Shipper()
            {
                ShipperID = 0
            };
            ViewBag.Title = "Thêm người giao hàng";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Shipper model)
        {
            //Kiểm tra lỗi đầu vào
            if (string.IsNullOrWhiteSpace(model.ShipperName))
            {
                ModelState.AddModelError("ShipperName", "Tên người giao hàng không được trống!");
            }

            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                model.Phone = "";
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ShipperID == 0 ? "Nhập lại thông tin người giao hàng" : "----";
                return View("Create", model);
            }

            if(model.ShipperID > 0)
            {
                CommonDataService.UpdateShipper(model);
            }
            else
            {
                CommonDataService.AddShipper(model);
            }

            Session["SHIPPER_SEARCH"] = new Models.PaginationSearchInput()
            {
                Page = 1,
                PageSize = 5,
                SearchValue = model.ShipperName
            };

            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        /// 
        [Route("edit/{shipperID}")]
        public ActionResult Edit(int shipperID)
        {
            Shipper model = CommonDataService.GetShipper(shipperID);
            if(model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title= "Cập nhật thông tin người giao hàng";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        /// 
        [Route("delete/{shipperID}")]
        public ActionResult Delete(int shipperID)
        {
            if(Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteShipper(shipperID);
                return RedirectToAction("Index");
            }

            Shipper model = CommonDataService.GetShipper(shipperID);
            if(model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }


}