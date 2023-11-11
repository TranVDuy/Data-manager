using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081005.Web.Models
{
    /// <summary>
    /// Lưu trữ kết quả tìm kiếm phân trang liên quan đến nhà cung cấp
    /// </summary>
    public class SupplierPaginationResult : BasePaginationResult
    {
        public List<Supplier> Data { get; set; }
    }
}