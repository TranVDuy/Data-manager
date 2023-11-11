using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081005.DataLayer.SQLServer
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAL(string connectionString) : base(connectionString)
        {

        }
        /// <summary>
        /// Bổ sung mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Product data)
        {
            int result = 0;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Products(ProductName, SupplierID, CategoryID, Unit, Price, Photo)
                                    VALUES (@productName, @supplierID, @categoryID, @unit, @price, @photo);
                                    SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@productName", data.ProductName);
                cmd.Parameters.AddWithValue("@supplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@categoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@unit", data.Unit);
                cmd.Parameters.AddWithValue("@price", data.Price);
                cmd.Parameters.AddWithValue("@photo", data.Photo);

                result = Convert.ToInt32(cmd.ExecuteScalar());


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Đếm mặt hàng dựa vào kết quả tìm kiếm
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue = "", int searchCategoryID = 0, int searchSupplierID = 0)
        {
            int count = 0;

            if (searchValue != "")
            {
                searchValue = "%" + searchValue + "%";
            }

            using (SqlConnection cn = OpenConnection())
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*)
                                    from   Products as p
                                   where	((@categoryID = 0) or (p.CategoryID = @categoryID))
	                                        and ((@supplierID = 0) or (p.SupplierID = @supplierID))
	                                        and ((@searchValue = N'') or (p.ProductName like @searchValue))";

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@supplierID", searchSupplierID);
                cmd.Parameters.AddWithValue("@categoryID", searchCategoryID);

                count = Convert.ToInt32(cmd.ExecuteScalar());


                cn.Close();
            }

            return count;
        }
        /// <summary>
        /// Xóa mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DELETE FROM Products WHERE ProductID = @productID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@productID", id);

                result = cmd.ExecuteNonQuery() > 0;


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Lấy thông tin một mặt hàng dựa vào mã mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product Get(int id)
        {
            Product data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Products WHERE ProductID = @productID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@productID", id);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    data = new Product()
                    {
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        ProductName = Convert.ToString(dbReader["ProductName"]),
                        SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                        CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                        Unit = Convert.ToString(dbReader["Unit"]),
                        Price = Convert.ToString(dbReader["Price"]),
                        Photo = Convert.ToString(dbReader["Photo"])
                    };
                }

                cn.Close();
            }

            return data;
        }
        /// <summary>
        /// Kiểm tra sự phụ thuộc của mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool InUsed(int id)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT CASE
	                                    WHEN EXISTS (SELECT * FROM OrderDetails WHERE ProductID = @productID) THEN 1
	                                    WHEN EXISTS (SELECT * FROM ProductAttributes WHERE ProductID = @productID) THEN 1
	                                    WHEN EXISTS (SELECT * FROM ProductPhotos WHERE ProductID = @productID) THEN 1
                                    ELSE 0 END";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@productID", id);


                result = Convert.ToBoolean(cmd.ExecuteScalar());


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Lấy danh sách tìm kiếm mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="seacrchCategoriesID"></param>
        /// <param name="searchSupplierID"></param>
        /// <returns></returns>
        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int searchCategoryID = 0, int searchSupplierID = 0)
        {
            List<Product> data = new List<Product>();
            if (searchValue != "")
            {
                searchValue = "%" + searchValue + "%";
            }

            //Tạo và mở kết nối
            using (SqlConnection cn = OpenConnection())
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                    from
                                        (
                                            select	p.*,
		                                            row_number() over(order by ProductName) as RowNumber
                                            from	Products as p
                                            where	((@categoryID = 0) or (p.CategoryID = @categoryID))
	                                            and ((@supplierID = 0) or (p.SupplierID = @supplierID))
	                                            and ((@searchValue = N'') or (p.ProductName like @searchValue))
                                        ) as t
                                    where (@pageSize = 0) or (t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                                    order by t.RowNumber;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@supplierID", searchSupplierID);
                cmd.Parameters.AddWithValue("@categoryID", searchCategoryID);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (result.Read())
                {
                    data.Add(new Product()
                    {
                        ProductID = Convert.ToInt32(result["ProductID"]),
                        ProductName = Convert.ToString(result["ProductName"]),
                        SupplierID = Convert.ToInt32(result["SupplierID"]),
                        CategoryID = Convert.ToInt32(result["CategoryID"]),
                        Unit = Convert.ToString(result["Unit"]),
                        Price = Convert.ToString(result["Price"]),
                        Photo = Convert.ToString(result["Photo"])

                    });
                }

                cn.Close();
            }

            return data;
        }

        /// <summary>
        /// Cập nhật thông tin mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Product data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Products
                                    SET   
	                                    ProductName = @productName, 
	                                    SupplierID = @supplierID, 
	                                    CategoryID = @categoryID, 
	                                    Unit = @unit, 
	                                    Price = @price, 
	                                    Photo = @photo
                                    WHERE ProductID = @productID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@productName", data.ProductName);
                cmd.Parameters.AddWithValue("@supplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@categoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@unit", data.Unit);
                cmd.Parameters.AddWithValue("@price", data.Price);
                cmd.Parameters.AddWithValue("@photo", data.Photo);
                cmd.Parameters.AddWithValue("@productID", data.ProductID);


                result = cmd.ExecuteNonQuery() > 0;


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Lấy danh sách thông tin chi tiết mặt hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        /// 
        


    }
}
