using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081005.Web.Models
{
    /// <summary>
    /// Lớp cơ sở (Lớp cha) của các lớp dùng để lưu trữ giữ các kết quả
    /// liên quan đển tìm kiếm, phân trang
    /// </summary>
    public abstract class BasePaginationResult
    {
        /// <summary>
        /// Trang hiện tại đang xem
        /// </summary>
        public int Page { set; get; }
        /// <summary>
        /// Số dòng hiển thị trên mỗi trang
        /// </summary>
        public int PageSize { set; get; }
        /// <summary>
        /// Giá trị tìm kiếm
        /// </summary>
        public string SearchValue { set; get; }
        /// <summary>
        /// Tổng số dòng tìm được
        /// </summary>
        public int RowCount { set; get; }
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PageCount
        {
            get
            {
                //if (PageSize == 0)
                //    return 1;
                int p = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    p += 1;
                return p;
            }
        }
    }
}