using SV19T1081005.DataLayer;
using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081005.DataLayer.SQLServer;

namespace SV19T1081005.BusinessLayer
{
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu về mặt hàng
    /// </summary>
    public class ProductDataService
    {
        private static readonly IProductDAL productDB;

        static ProductDataService()
        {
            string provider = ConfigurationManager
                                    .ConnectionStrings["DB"]
                                    .ProviderName;
            string connectionString = ConfigurationManager
                                            .ConnectionStrings["DB"]
                                            .ConnectionString;

            switch (provider)
            {
                case "SQLServer":
                    productDB = new DataLayer.SQLServer.ProductDAL(connectionString);
                    break;

                default:
                    //categoryDB = new DataLayer.FakeDB.CategoryDAL();
                    break;
            }
        }
        #region các chức năng liên quan đến mặt hàng
        /// <summary>
        /// Lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="CategoryID"></param>
        /// <param name="SupplierID"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Product> ListOfProducts(int page 
                                                    , int pageSize
                                                    , string searchValue
                                                    , int CategoryID 
                                                    , int SupplierID
                                                    , out int rowCount)
        {
            rowCount = productDB.Count(searchValue, CategoryID, SupplierID);
            return productDB.List(page, pageSize, searchValue, CategoryID, SupplierID).ToList();
        }
        /// <summary>
        /// Cập nhật dữ liệu mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }
        /// <summary>
        /// Bổ sung mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }
        /// <summary>
        /// Lấy thông tin một mặt hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static Product GetProduct(int productID)
        {
            return productDB.Get(productID);
        }
        /// <summary>
        /// Kiểm tra sự phụ thuộc của mặt hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static bool InUsedProduct(int productID)
        {
            return productDB.InUsed(productID);
        }
        /// <summary>
        /// Xóa mặt hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static bool DeleteProduct(int productID)
        {
            return productDB.Delete(productID);
        }

        #endregion

        
    }
}
