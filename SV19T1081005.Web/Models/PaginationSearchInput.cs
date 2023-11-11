using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081005.Web.Models
{
    /// <summary>
    /// Dữ liệu đầu vào để tìm kiếm, phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; }
    }

    public class ProductSearchInput : PaginationSearchInput
    {
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
    }
}