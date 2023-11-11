using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081005.DataLayer;
using SV19T1081005.DomainModel;

namespace SV19T1081005.BusinessLayer
{
    /// <summary>
    /// Các chức năng xử lý về thư viện ảnh, thuộc tính của mặt hàng
    /// </summary>
    public class Product__DataService
    {
        private static readonly IProduct__DAL<ProductAttribute> productattributeDB;
        private static readonly IProduct__DAL<ProductPhoto> productphotoDB;

        /// <summary>
        /// 
        /// </summary>
        static Product__DataService()
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
                    productattributeDB = new DataLayer.SQLServer.ProductAttributeDAL(connectionString);
                    productphotoDB = new DataLayer.SQLServer.ProductPhotoDAL(connectionString);
                    break;

                default:
                    //categoryDB = new DataLayer.FakeDB.CategoryDAL();
                    break;
            }


        }

        #region các chức năng liên quan đến thuộc tính của mặt hàng
        /// <summary>
        /// Lấy danh sách các thuộc tính
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static List<ProductAttribute> ListProductAttributes(int productID)
        {
            return productattributeDB.List(productID).ToList();
        }
        /// <summary>
        /// Bổ sung thuộc tính
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddProductAttribute(ProductAttribute data)
        {
            return productattributeDB.Add(data);
        }
        /// <summary>
        /// Lấy thông tin của một thuộc tính
        /// </summary>
        /// <param name="productAttributeID"></param>
        /// <returns></returns>
        public static ProductAttribute GetProductAttribute(int productAttributeID)
        {
            return productattributeDB.Get(productAttributeID);
        }
        /// <summary>
        /// Kiểm tra thứ tự hiển thị đã tồn tại hay chưa
        /// </summary>
        /// <param name="DisplayOrder"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static bool CheckDisplayOrderProductAttribute(int DisplayOrder, int ProductID)
        {
            return productattributeDB.Check(DisplayOrder, ProductID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AttributeID"></param>
        /// <param name="DisplayOrder"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static bool CheckProductAttribute(long AttributeID, int DisplayOrder, int ProductID)
        {
            return productattributeDB.Check(AttributeID, DisplayOrder, ProductID);
        }
        /// <summary>
        /// Cập nhật thông tin thuộc tính
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProductAttribute(ProductAttribute data)
        {
            return productattributeDB.Update(data);
        }
        /// <summary>
        /// Xóa thuộc tính của mặt hàng
        /// </summary>
        /// <param name="pdattributeID"></param>
        /// <returns></returns>
        public static bool DeleteProductAttribute(long productattributeID)
        {
            return productattributeDB.Delete(productattributeID);
        }
        #endregion

        #region các chức năng liên quan đến thư viện ảnh của mặt hàng
        /// <summary>
        /// Lấy danh sách thư viện ảnh
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static List<ProductPhoto> ListProductPhotos(int productID)
        {
            return productphotoDB.List(productID).ToList();
        }
        /// <summary>
        /// Kiểm tra thứ tự hiện thị ảnh khi thêm ảnh
        /// </summary>
        /// <param name="DisplayOrder"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static bool CheckProductPhoto(int DisplayOrder, int ProductID)
        {
            return productphotoDB.Check(DisplayOrder, ProductID);
        }
        /// <summary>
        /// Kiểm tra thứ tự hiển thị ảnh khi chỉnh sửa thông tin ảnh
        /// </summary>
        /// <param name="PhotoID"></param>
        /// <param name="DisplayOrder"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static bool CheckProductPhoto(long PhotoID, int DisplayOrder, int ProductID)
        {
            return productphotoDB.Check(PhotoID, DisplayOrder, ProductID);
        }
        /// <summary>
        /// Lấy thông tin của một ảnh dựa vào ID
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public static ProductPhoto GetProductPhoto(long photoID)
        {
            return productphotoDB.Get(photoID);
        }
        /// <summary>
        /// xóa một ảnh dựa vào ID
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public static bool DeleteProductPhoto(long photoID)
        {
            return productphotoDB.Delete(photoID);
        }
        /// <summary>
        /// Bổ sung ảnh vào thư viện
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddProductPhoto(ProductPhoto data)
        {
            return productphotoDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin ảnh
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProductPhoto(ProductPhoto data)
        {
            return productphotoDB.Update(data);
        }
        #endregion
    }
}
