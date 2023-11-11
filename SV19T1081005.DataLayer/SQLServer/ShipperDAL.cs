using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081005.DataLayer.SQLServer
{
    public class ShipperDAL : _BaseDAL, ICommonDAL<Shipper>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public ShipperDAL(string connectionString) : base(connectionString)
        {

        }
        /// <summary>
        /// Bổ sung người giao hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Shipper data)
        {
            int result = 0;
           

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Shippers(ShipperName, Phone)
                                    VALUES (@shipperName, @phone);
                                    SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@shipperName", data.ShipperName);
                cmd.Parameters.AddWithValue("@phone", data.Phone);
                

                result = Convert.ToInt32(cmd.ExecuteScalar());


                cn.Close();
            }
            return result;
            
        }
        /// <summary>
        /// Đếm người giao hàng dựa vào kết quả tìm kiếm
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
                                    from    Shippers
                                    where  (@searchValue = N'')
                                        or (
                                                (ShipperName like @searchValue)
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
        /// Xóa người giao hàng dựa vào ID
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public bool Delete(int shipperID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DELETE FROM Shippers WHERE ShipperID = @shipperID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@shipperID", shipperID);

                result = cmd.ExecuteNonQuery() > 0;


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Lấy thông tin của một người giao hàng dựa vào ID
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public Shipper Get(int shipperID)
        {
            Shipper data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Shippers WHERE ShipperID = @shipperID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@shipperID", shipperID);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    data = new Shipper()
                    {
                        ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                        ShipperName = Convert.ToString(dbReader["ShipperName"]),
                        Phone = Convert.ToString(dbReader["Phone"]),
                       
                    };
                }

                cn.Close();
            }

            return data;
        }
        /// <summary>
        /// Kiểm tra sự phụ thuộc của người giao hàng
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public bool InUsed(int shipperID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT COUNT(*)
                                    FROM Shippers as s
	                                        RIGHT JOIN Orders as o ON s.ShipperID = o.ShipperID
                                    WHERE s.ShipperID = @shipperID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@shipperID", shipperID);


                result = (Convert.ToInt32(cmd.ExecuteScalar()) > 0);


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Lấy danh sách người giao hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public IList<Shipper> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Shipper> data = new List<Shipper>();

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
                                                    row_number() over(order by ShipperName) as RowNumber
                                            from    Shippers
                                            where    (@searchValue = N'')
                                                or (
                                                        (ShipperName like @searchValue)
                                                        or
                                                        (Phone like @searchValue)
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
                    data.Add(new Shipper()
                    {
                        ShipperID = Convert.ToInt32(result["ShipperID"]),
                        ShipperName = Convert.ToString(result["ShipperName"]),
                        Phone = Convert.ToString(result["Phone"])
                       
                    }); 
                }

                cn.Close();
            }
            return data;
        }
        /// <summary>
        /// Cập nhật thông tin người giao hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Shipper data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Shippers
                                    SET ShipperName = @shipperName,
	                                    Phone = @phone
                                    WHERE ShipperID = @shipperID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@shipperName", data.ShipperName);
                cmd.Parameters.AddWithValue("@phone", data.Phone);
                cmd.Parameters.AddWithValue("@shipperID", data.ShipperID);
 
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
