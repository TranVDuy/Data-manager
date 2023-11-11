using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081005.DataLayer.SQLServer
{
   
    /// <summary>
    /// 
    /// </summary>
    public class CustomerDAL : _BaseDAL, ICommonDAL<Customer>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public CustomerDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Thêm khách hàng kết quả trả về ID khách hàng được thêm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Customer data)
        {
            int result = 0;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Customers(CustomerName, ContactName, Address, City, PostalCode, Country)
                            VALUES (@customerName, @contactName, @address, @city, @postalCode, @country);
                            SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@customerName", data.CustomerName);
                cmd.Parameters.AddWithValue("@contactName", data.ContactName);
                cmd.Parameters.AddWithValue("@address", data.Address);
                cmd.Parameters.AddWithValue("@city", data.City);
                cmd.Parameters.AddWithValue("@postalCode", data.PostalCode);
                cmd.Parameters.AddWithValue("@country", data.Country);

                result = Convert.ToInt32(cmd.ExecuteScalar());


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Đếm khách hàng dựa vào kết quả tìm kiếm
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
                                    from    Customers
                                    where  (@searchValue = N'')
                                        or (
                                                (CustomerName like @searchValue)
                                                or
                                                (ContactName like @searchValue)
                                                or
                                                (Address like @searchValue)
                                                or
                                                (PostalCode like @searchValue)
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
        /// Xóa một khách hàng dựa vào ID
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public bool Delete(int CustomerID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DELETE FROM Customers WHERE CustomerID = @customerID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@customerID", CustomerID);

                result = cmd.ExecuteNonQuery() > 0;

                
                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Lấy thông tin một khách hàng dựa vào ID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Customer Get(int customerID)
        {
            Customer data = null;

            using(SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Customers WHERE CustomerID = @customerID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@customerID", customerID);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    data = new Customer()
                    {
                        CustomerID = Convert.ToInt32(dbReader["CustomerID"]),
                        CustomerName = Convert.ToString(dbReader["CustomerName"]),
                        Address = Convert.ToString(dbReader["Address"]),
                        City = Convert.ToString(dbReader["City"]),
                        ContactName = Convert.ToString(dbReader["ContactName"]),
                        Country = Convert.ToString(dbReader["Country"]),
                        PostalCode = Convert.ToString(dbReader["PostalCode"])
                    };
                }

                cn.Close();
            }

            return data;
        }
        /// <summary>
        /// Kiểm tra sự phụ thuộc của khách hàng với đơn hàng dựa vào ID khach hàng
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public bool InUsed(int CustomerID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT CASE WHEN EXISTS(SELECT * FROM Orders WHERE CustomerID = @customerID) THEN 1 ELSE 0 END";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@customerID", CustomerID);
                

                result = Convert.ToBoolean(cmd.ExecuteScalar());


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// lấy danh sách khách hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public IList<Customer> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Customer> data = new List<Customer>();

            if(searchValue != "")
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
                                                    row_number() over(order by CustomerName) as RowNumber
                                            from    Customers
                                            where    (@searchValue = N'')
                                                or (
                                                        (CustomerName like @searchValue)
                                                        or
                                                        (ContactName like @searchValue)
                                                        or
                                                        (Address like @searchValue)
                                                        or
                                                        (PostalCode like @searchValue)
                                                    )
                                        ) as t
                                    where  (@pageSize = 0) or (t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                                    order by t.RowNumber;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (result.Read())
                {
                    data.Add(new Customer()
                    {
                        CustomerID = Convert.ToInt32(result["CustomerID"]),
                        CustomerName = Convert.ToString(result["CustomerName"]),
                        Address = Convert.ToString(result["Address"]),
                        City = Convert.ToString(result["City"]),
                        ContactName = Convert.ToString(result["ContactName"]),
                        Country = Convert.ToString(result["Country"]),
                        PostalCode = Convert.ToString(result["PostalCode"])
                    }); 
                }

                cn.Close();
            }
            return data;
        }
        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Customer data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Customers
                                    SET CustomerName = @customerName, 
	                                    ContactName = @contactName, 
	                                    Address = @address, 
	                                    City = @city,
	                                    PostalCode = @postalCode,
	                                    Country = @Country
                                    WHERE CustomerID = @customerID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@customerName", data.CustomerName);
                cmd.Parameters.AddWithValue("@contactName", data.ContactName);
                cmd.Parameters.AddWithValue("@address", data.Address);
                cmd.Parameters.AddWithValue("@city", data.City);
                cmd.Parameters.AddWithValue("@postalCode", data.PostalCode);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@customerID", data.CustomerID);
               

                result = cmd.ExecuteNonQuery() > 0;


                cn.Close();
            }

            return result;
        }
    }
}
