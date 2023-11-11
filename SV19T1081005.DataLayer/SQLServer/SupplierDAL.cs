using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081005.DataLayer.SQLServer
{
    public class SupplierDAL : _BaseDAL, ICommonDAL<Supplier>
    {
        public SupplierDAL(string connectionString) : base(connectionString)
        {
        }
        /// <summary>
        /// Bổ sung nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Supplier data)
        {
            int result = 0;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Suppliers(SupplierName, ContactName, Address, City, PostalCode, Country, Phone)
                                    VALUES (@supplierName, @contactName, @address, @city, @postalCode, @country, @phone);
                                    SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@supplierName", data.SupplierName);
                cmd.Parameters.AddWithValue("@contactName", data.ContactName);
                cmd.Parameters.AddWithValue("@address", data.Address);
                cmd.Parameters.AddWithValue("@city", data.City);
                cmd.Parameters.AddWithValue("@postalCode", data.PostalCode);
                cmd.Parameters.AddWithValue("@country", data.Country);
                cmd.Parameters.AddWithValue("@phone", data.Phone);

                result = Convert.ToInt32(cmd.ExecuteScalar());


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Đếm nhà cung cấp dựa vào kết quả tìm kiếm
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
                                    from    Suppliers
                                    where  (@searchValue = N'')
                                        or (
                                                (SupplierName like @searchValue)
                                                or
                                                (ContactName like @searchValue)
                                                or
                                                (Address like @searchValue)
                                                or
                                                (Phone like @searchValue)
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
        /// Xóa nhà cung cấp
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        public bool Delete(int SupplierID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DELETE FROM Suppliers WHERE SupplierID = @supplierID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@supplierID", SupplierID);

                result = cmd.ExecuteNonQuery() > 0;


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Lấy thông tin của nhà cung cấp dựa vào mã nhà cung cấp
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public Supplier Get(int supplierID)
        {
            Supplier data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Suppliers WHERE SupplierID = @supplierID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@supplierID", supplierID);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    data = new Supplier()
                    {
                        SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                        SupplierName = Convert.ToString(dbReader["SupplierName"]),
                        Address = Convert.ToString(dbReader["Address"]),
                        City = Convert.ToString(dbReader["City"]),
                        ContactName = Convert.ToString(dbReader["ContactName"]),
                        Country = Convert.ToString(dbReader["Country"]),
                        PostalCode = Convert.ToString(dbReader["PostalCode"]),
                        Phone = Convert.ToString(dbReader["Phone"])
                    };
                }

                cn.Close();
            }

            return data;
        }
        /// <summary>
        /// Kiểm tra sự phụ thuộc của nhà cung cấp
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        public bool InUsed(int SupplierID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT COUNT (*)
                                    FROM Suppliers as s
	                                    RIGHT JOIN Products as p ON s.SupplierID = p.SupplierID
                                    WHERE s.SupplierID = @supplierID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@supplierID", SupplierID);


                result = (Convert.ToInt32(cmd.ExecuteScalar()) > 0);


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Lấy danh sách nhà cung cấp
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public IList<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Supplier> data = new List<Supplier>();

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
                                                    row_number() over(order by SupplierName) as RowNumber
                                            from    Suppliers
                                            where    (@searchValue = N'')
                                                or (
                                                        (SupplierName like @searchValue)
                                                        or
                                                        (ContactName like @searchValue)
                                                        or
                                                        (Address like @searchValue)
                                                        or
                                                        (Phone like @searchValue)
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
                    data.Add(new Supplier()
                    {
                        SupplierID = Convert.ToInt32(result["SupplierID"]),
                        SupplierName = Convert.ToString(result["SupplierName"]),
                        Address = Convert.ToString(result["Address"]),
                        City = Convert.ToString(result["City"]),
                        ContactName = Convert.ToString(result["ContactName"]),
                        Country = Convert.ToString(result["Country"]),
                        PostalCode = Convert.ToString(result["PostalCode"]),
                        Phone = Convert.ToString(result["Phone"])
                    }); ;
                }

                cn.Close();
            }
            return data;
        }
        /// <summary>
        /// Cập nhật thông tin nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Supplier data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Suppliers
                                    SET SupplierName = @supplierName, 
	                                    ContactName = @contactName, 
	                                    Address = @address, 
	                                    City = @city,
	                                    PostalCode = @postalCode,
	                                    Country = @Country,
	                                    Phone = @phone
                                    WHERE SupplierID = @supplierID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@supplierName", data.SupplierName);
                cmd.Parameters.AddWithValue("@contactName", data.ContactName);
                cmd.Parameters.AddWithValue("@address", data.Address);
                cmd.Parameters.AddWithValue("@city", data.City);
                cmd.Parameters.AddWithValue("@postalCode", data.PostalCode);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@phone", data.Phone);
                cmd.Parameters.AddWithValue("@supplierID", data.SupplierID);


                result = cmd.ExecuteNonQuery() > 0;


                cn.Close();
            }

            return result;
        }

    }   
}
