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
    [RoutePrefix("supplier")]
    public class SupplierController : Controller
    {
        // GET: Supplier
        public ActionResult Index()
        {
            //Lấy điều kiện tìm kiếm từ trong session
            Models.PaginationSearchInput model = Session["SUPPLIER_SEARCH"] as Models.PaginationSearchInput;

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
        /// <summary>
        /// Tìm kiếm từ mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        [Route("searchfromproduct/{supplierID}")]
        public ActionResult SearchFromProduct(string method, string supplierID)
        {

            Supplier sp = CommonDataService.GetSupplier(Convert.ToInt32(supplierID));
            Session["SUPPLIER_SEARCH"] = new Models.PaginationSearchInput()
            {
                Page = 1,
                PageSize = 10,
                SearchValue = sp.SupplierName
            };

            return RedirectToAction("Index");
        }
        public ActionResult Search(Models.PaginationSearchInput input)
        {

            int rowCount = 0;

            var data = BusinessLayer.CommonDataService.ListOfSuppliers(input.Page,
                                                                       input.PageSize,
                                                                       input.SearchValue,
                                                                       out rowCount);
            Models.SupplierPaginationResult model = new Models.SupplierPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm vào session
            Session["SUPPLIER_SEARCH"] = input;

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public ActionResult Create()
        {
            Supplier model = new Supplier()
            {
                SupplierID = 0
            };
            ViewBag.Title = "Thêm nhà cung cấp";
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(Supplier model)
        {
            //Kiểm tra dữ liệu đầu vào nhà cung cấp
            if (string.IsNullOrWhiteSpace(model.SupplierName))
            {
                ModelState.AddModelError("SupplierName", "Tên nhà cung cấp không được trống");
            }
            if (string.IsNullOrWhiteSpace(model.ContactName))
            {
                ModelState.AddModelError("ContactName", "Tên giao dịch không được trống");
            }

            if (string.IsNullOrWhiteSpace(model.Address))
                ModelState.AddModelError("Address", "Địa chỉ không được để trống"); ;
            if (string.IsNullOrWhiteSpace(model.Country))
                ModelState.AddModelError("Country", "Phải chọn quốc gia");
            if (string.IsNullOrWhiteSpace(model.PostalCode))
                model.PostalCode = "";
            if (string.IsNullOrWhiteSpace(model.City))
                model.City = "";
            if (string.IsNullOrWhiteSpace(model.Phone))
                model.Phone = "";

            //true nếu modelstate không có lỗi nào (kiểm tra hợp lệ)
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.SupplierID == 0 ? "Hợp lệ" : "Cập nhật lại thông tin nhà cung cấp";
                return View("Create", model);
            }


            //Cập nhập dữ liệu
            if (model.SupplierID > 0)
            {
                CommonDataService.UpdateSupplier(model);
            }
            else
            {
                CommonDataService.AddSupplier(model);
            }

            Session["SUPPLIER_SEARCH"] = new Models.PaginationSearchInput()
            {
                Page = 1,
                PageSize = 10,
                SearchValue = model.SupplierName
            };


            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        /// 
        [Route("delete/{supplierID}")]
        public ActionResult Delete(int supplierID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteSupplier(supplierID);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetSupplier(supplierID);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        /// 
        [Route("edit/{supplierID}")]
        public ActionResult Edit(int supplierID)
        {
            Supplier model = CommonDataService.GetSupplier(supplierID);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin nhà cung cấp";
            return View("Create", model);
        }
    }
}