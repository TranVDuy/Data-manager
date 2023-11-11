using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SV19T1081005.DataLayer.SQLServer
{
    public class CategoryDAL : _BaseDAL, ICommonDAL<Category>
    {
        //private string connectionString;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="connectionString"></param>
        public CategoryDAL(string connectionString) : base(connectionString)
        {

        }

        /// <summary>
        /// Bổ sung loại hàng loại hàng, kết quả trả về ID của loại hàng vừa thêm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Category data)
        {
            int result = 0;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Categories(CategoryName, Description)
                                    VALUES (@categoryName, @description);
                                    SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@categoryName", data.CategoryName);
                cmd.Parameters.AddWithValue("@description", data.Description);
               

                result = Convert.ToInt32(cmd.ExecuteScalar());


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Đếm loại hàng dựa vào kết quả tìm kiếm
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue = "")
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
                                    from   Categories
                                    where  (@searchValue = N'')
                                        or (
                                                (CategoryName like @searchValue)
                                                or
                                                (Description like @searchValue)
                                               
                                            )";

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());


                cn.Close();
            }

            return count;
        }

        /// <summary>
        /// Xóa loại hàng
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public bool Delete(int categoryID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DELETE FROM Categories WHERE CategoryID =  @categoryID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@categoryID", categoryID);

                result = cmd.ExecuteNonQuery() > 0;


                cn.Close();
            }
            return result;
        }

        /// <summary>
        /// Lấy thông tin của một loại hàng dựa vào mã loại hàng
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public Category Get(int categoryID)
        {
            Category data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Categories WHERE CategoryID = @categoryID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@categoryID", categoryID);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    data = new Category()
                    {
                        CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                        CategoryName = Convert.ToString(dbReader["CategoryName"]),
                        Description = Convert.ToString(dbReader["Description"])
                        
                    };
                }

                cn.Close();
            }

            return data;
        }
        /// <summary>
        /// Kiểm tra sự phụ thuộc của loại hàng
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public bool InUsed(int categoryID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT COUNT(*)
                                    FROM Categories as c
	                                        RIGHT JOIN Products as p ON c.CategoryID = p.CategoryID
                                    WHERE c.CategoryID = @categoryID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@categoryID", categoryID);


                result = (Convert.ToInt32(cmd.ExecuteScalar()) > 0);


                cn.Close();
            }

            return result;
        }

        /// <summary>
        /// Lấy danh sách loại hàng dựa vào kết quả tìm kiếm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>

        public IList<Category> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Category> data = new List<Category>();

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
                                            select    *,
                                                    row_number() over(order by CategoryName) as RowNumber
                                            from    Categories
                                            where    (@searchValue = N'')
                                                or (
                                                        (CategoryName like @searchValue)
                                                        or
                                                        (Description like @searchValue)
                                                    )
                                        ) as t
                                    where (@pageSize = 0) or (t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                                    order by t.RowNumber;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (result.Read())
                {
                    data.Add(new Category()
                    {
                        CategoryID = Convert.ToInt32(result["CategoryID"]),
                        CategoryName = Convert.ToString(result["CategoryName"]),
                        Description = Convert.ToString(result["Description"])

                    });
                }

                cn.Close();
            }
            return data;
        }

        /// <summary>
        /// Cập nhật thông tin loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Category data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText =  @"UPDATE Categories
                                    SET CategoryName = @categoryName,
	                                    Description =  @description
                                    WHERE CategoryID = @categoryID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@categoryName", data.CategoryName);
                cmd.Parameters.AddWithValue("@description", data.Description);
                
                cmd.Parameters.AddWithValue("@categoryID", data.CategoryID);


                result = cmd.ExecuteNonQuery() > 0;


                cn.Close();
            }

            return result;
        }
    }
}
