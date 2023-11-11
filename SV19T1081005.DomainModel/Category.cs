using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081005.DomainModel
{
    /// <summary>
    /// Loại hàng
    /// </summary>

    public class Category
    {
        /// <summary>
        /// Mã loại hàng
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Tên loại hàng
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// Mô tả loại hàng
        /// </summary>
        ///THu gọn: ctrl + M + O, mở ra: ctrl + M + L 

        public string Description { get; set; }
    }
}
