using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SV19T1081005.BusinessLayer;
using SV19T1081005.DomainModel;

namespace SV19T1081005.Web.Controllers
{
    [Authorize]
    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("Email", "Email không được để trống");
            }
            else
            {
                ViewBag.Email = email;
            }

            if (string.IsNullOrWhiteSpace(password))
                ModelState.AddModelError("Password", "Mật khẩu không được để trống");

            if (!ModelState.IsValid)
            {
                //ViewBag.Title = model.CustomerID == 0 ? "Bổ sung khách hàng" : "Cập nhật khách hàng";
                return View();
            }


            if (AccountDataService.Check(email, password))
            {
                var account = AccountDataService.GetAccount(email, password);
                System.Web.Security.FormsAuthentication.SetAuthCookie(email, false);
                Session["Photo"] = account.Photo;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Email = email;
                ViewBag.Message = "Email hoặc mật khẩu không đúng";
                return View();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [Route("changepassword")]
        public ActionResult ChangePassword(AccountEmployee model)
        {
            return View(model);
            //return RedirectToAction("notify");
        }
        [HttpPost]
        public ActionResult SaveChange(AccountEmployee model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Email", "Email không được để trống");
            else
                if (model.Email != User.Identity.Name)
                {
                    ViewBag.Email = model.Email.Trim();
                    ModelState.AddModelError("Email", "Email không hợp lệ!");
                }
                else
                    ViewBag.Email = model.Email.Trim();

            if (string.IsNullOrWhiteSpace(model.Password))
                ModelState.AddModelError("Password", "Mật khẩu không được để trống!");
            else
                if (!AccountDataService.Check(model.Email, model.Password))
                ModelState.AddModelError("Password", "Mật khẩu không đúng!");

            if (string.IsNullOrWhiteSpace(model.NewPassword))
                ModelState.AddModelError("NewPassword", "Mật khẩu mới trống");
            else
            {
                if (model.NewPassword.Trim().Equals(model.Password.Trim()))
                {
                    //ViewBag.NewPassword = "Mật khẩu mới phải khác mật khẩu cũ!";
                    ModelState.AddModelError("NewPassword", "Mật khẩu mới phải khác mật khẩu cũ");
                    ViewBag.NewPass = model.NewPassword.Trim();
                }

            }

            if (!ModelState.IsValid)
            {
                return View("ChangePassword", model);
            }


            if (AccountDataService.UpdateAccount(model))
                return RedirectToAction("Notify");


            return View();
        }

        public ActionResult Notify()
        {
            return View();
        }
    }
}