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
    [RoutePrefix("customer")]
    public class CustomerController : Controller
    {
       /// <summary>
       /// -----------
       /// </summary>
       /// <returns></returns>
        public ActionResult Index()
        {
            //Lấy điều kiện tìm kiếm từ trong session
            Models.PaginationSearchInput model = Session["CUSTOMER_SEARCH"] as Models.PaginationSearchInput;

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
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult Search(Models.PaginationSearchInput input)
        {
            
            int rowCount = 0;

            var data = BusinessLayer.CommonDataService.ListOfCustomers(input.Page,
                                                                       input.PageSize,
                                                                       input.SearchValue,
                                                                       out rowCount);
            Models.CustomerPaginationResult model = new Models.CustomerPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm vào session
            Session["CUSTOMER_SEARCH"] = input;

            return View(model);
        }
        /// <summary>
        /// Tạo
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Customer model = new Customer()
            {
                CustomerID = 0
            };
            ViewBag.Title = "Bổ sung khách hàng";
            return View(model);
        }
        /// <summary>
        /// Chỉnh sửa
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        /// 

        [Route("edit/{customerID}")]
        public ActionResult Edit(int customerID)
        {
            Customer model = CommonDataService.GetCustomer(customerID);
            if(model == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Cập nhật thông tin khách hàng";
            return View("Create", model);
        }

        /// <summary>
        /// Lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Customer model)
        {
            //TODO: Kiểm tra dữ liệu đầu vào khách hàng
            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                ModelState.AddModelError("CustomerName", "Tên khách hàng không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.ContactName))
                ModelState.AddModelError("ContactName", "Tên giao dịch không được để trống");
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                ModelState.AddModelError("Address", "Địa chỉ không được trống");
            }
            if (string.IsNullOrWhiteSpace(model.Country))
                ModelState.AddModelError("Country", "Phải chọn quốc gia");
            if (string.IsNullOrWhiteSpace(model.City))
                model.City = "";
            if (string.IsNullOrWhiteSpace(model.PostalCode))
                model.PostalCode = "";

            //true nếu modelstate không có lỗi nào (kiểm tra hợp lệ)
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CustomerID == 0 ? "Bổ sung khách hàng" : "Cập nhật khách hàng";
                return View("Create", model);
            }

            if(model.CustomerID > 0)
            {
                CommonDataService.UpdateCustomer(model);
            }
            else
            {
                CommonDataService.AddCustomer(model);
            }

            Models.PaginationSearchInput ms = new Models.PaginationSearchInput()
            {
                Page = 1,
                PageSize = 10,
                SearchValue = model.CustomerName
            };

            Session["CUSTOMER_SEARCH"] = ms;
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        /// 

        [Route("delete/{customerID}")]
        public ActionResult Delete(int customerID)
        {
            if(Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCustomer(customerID);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetCustomer(customerID);
            if(model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}