using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081005.Web.Models
{
    /// <summary>
    /// Lưu trữ kết quả tìm kiếm phân trang liên quan đến khách hàng
    /// </summary>
    public class CustomerPaginationResult : BasePaginationResult
    {
        /// <summary>
        /// Danh sách các khách hàng
        /// </summary>
        public List<Customer> Data { get; set; }

    }
}