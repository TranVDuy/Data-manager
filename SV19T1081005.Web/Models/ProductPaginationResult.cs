using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081005.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductPaginationResult : BasePaginationResult
    {

        /// <summary>
        /// Danh sách các poducts
        /// </summary>
        public List<Product> Data { get; set; }

        /// <summary>
        /// danh sách categories
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// danh sách supplier
        /// </summary>
        public int SupplierID { get; set; }

        /// <summary>
        /// mã sản phẩm
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// đơn vị bán
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// tên sản phẩm
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// giá sản phẩm
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// ảnh sản phẩm
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Chứa 1 sản phẩm
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// List chứa các thuộc tính của sản phẩm
        /// </summary>
        public List<ProductAttribute> ListProductAttribute { get; set; }


        /// <summary>
        /// List chưa các ảnh của sản phẩm
        /// </summary>
        public List<ProductPhoto> ListProductPhoto { get; set; }



    }
}