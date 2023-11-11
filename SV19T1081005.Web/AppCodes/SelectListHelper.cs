using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SV19T1081005.DomainModel;
using SV19T1081005.BusinessLayer;
using System.Web.Mvc;

namespace SV19T1081005.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách quốc gia
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Countries()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "---Chọn quốc gia---"
            });
            foreach(var country in CommonDataService.ListOfCountries())
            {
                list.Add(new SelectListItem()
                {
                    Value = country.CountryName,
                    Text = country.CountryName
                });
            }

            return list;
        }

        /// <summary>
        /// Danh sách loại hàng
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "---Chọn loại hàng---"
            });
            foreach (var category in CommonDataService.ListOfCategories())
            {
                list.Add(new SelectListItem()
                {
                    Value = category.CategoryID.ToString(),
                    Text = category.CategoryName
                });
            }

            return list;
        }

        /// <summary>
        /// Danh sách nhà cung cấp
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Suppliers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "---Chọn nhà cung cấp---"
            });
            foreach (var supplier in CommonDataService.ListOfSuppliers())
            {
                list.Add(new SelectListItem()
                {
                    Value = supplier.SupplierID.ToString(),
                    Text = supplier.SupplierName
                });
            }

            return list;
        }
    }
}