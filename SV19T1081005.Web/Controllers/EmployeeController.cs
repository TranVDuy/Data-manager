using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SV19T1081005.BusinessLayer;
using System.Globalization;

namespace SV19T1081005.Web.Controllers
{
    [Authorize]
    [RoutePrefix("employee")]
    public class EmployeeController : Controller
    {
        // GET: Employee

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Lấy điều kiện tìm kiếm từ trong session
            Models.PaginationSearchInput model = Session["EMPLOYEE_SEARCH"] as Models.PaginationSearchInput;

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

            var data = BusinessLayer.CommonDataService.ListOfEmployees(input.Page,
                                                                       input.PageSize,
                                                                       input.SearchValue,
                                                                       out rowCount);
            Models.EmployeePaginationResult model = new Models.EmployeePaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm vào session
            Session["EMPLOYEE_SEARCH"] = input;

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("create")]
        public ActionResult Create()
        {
            Employee model = new Employee()
            {
                EmployeeID = 0
            };

            ViewBag.Title = "Thêm Nhân viên";
            return View(model);
        }

        public ActionResult Save(Employee model, string birthDateString, HttpPostedFileBase uploadPhoto)
        {

            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                ModelState.AddModelError("FirstName", "Họ đệm không được trống!");
            }
            if (string.IsNullOrWhiteSpace(model.LastName))
            {
                ModelState.AddModelError("LastName", "Tên nhân viên không được trống!");
            }
            if (model.BirthDate == null)
                ModelState.AddModelError("BirthDate", "Phải chọn ngày sinh!");

            if (string.IsNullOrWhiteSpace(model.Notes))
                model.Notes = "";

            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Email", "Email không được trống!");
            else
            {
                if(BusinessLayer.AccountDataService.CheckEmailExits(model.Email.Trim()))
                    ModelState.AddModelError("Email", "Email này đã được sử dụng, hãy thử một email khác!");

            }

            //Xử lý ngày sinh

            DateTime birthDay = DateTime.ParseExact(birthDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if(birthDay.Year < 1753 || birthDay.Year > 9999)
            {
                ModelState.AddModelError("BirthDate", "Ngày sinh không hợp lệ!");

            }

            model.BirthDate = birthDay;

            //Xử lý ảnh

            if (uploadPhoto != null)
            {
                string path = Server.MapPath("~/images/employees");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string uploadFilePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(uploadFilePath);
                model.Photo = $"/images/employees/{fileName}";
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.EmployeeID == 0 ? "Bổ sung nhân viên" : "";
                return View("Create", model);
            }
            
          /* return Json(new
             {
                 Model = model,
                 BirthDateString = birthDateString,
                 UploadPhoto = uploadPhoto.FileName
             });*/


            if (model.EmployeeID > 0)
            {
                CommonDataService.UpdateEmployee(model);
            }
            else
            {
                CommonDataService.AddEmployee(model);
            }

            Session["EMPLOYEE_SEARCH"] = new Models.PaginationSearchInput()
            {
                Page = 1,
                PageSize = 10,
                SearchValue = model.FirstName != "" ? model.FirstName : model.LastName
            };

            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        /// 
        [Route("delete/{employeeID}")]
        public ActionResult Delete(int employeeID)
        {

            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteEmployee(employeeID);
                return RedirectToAction("Index");
            }

            Employee model = CommonDataService.GetEmployee(employeeID);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        /// 
        [Route("edit/{employeeID}")]
        public ActionResult Edit(int employeeID)
        {
            Employee model = CommonDataService.GetEmployee(employeeID);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Cập nhật thông tin nhân viên";
            return View("Create", model);
        }
    }
}