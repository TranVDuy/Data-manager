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
    public class EmployeeDAL : _BaseDAL, ICommonDAL<Employee>
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {

        }
        /// <summary>
        /// Bổ sung nhân viên, kết quả trả về mã nhân viên đó
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Employee data)
        {
            int result = 0;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Employees(LastName, FirstName, BirthDate, Photo, Notes, Email)
                                    VALUES (@lastName, @firstName, @birthDate, @photo, @notes, @email);
                                    SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@lastName", data.LastName);
                cmd.Parameters.AddWithValue("@firstName", data.FirstName);
                cmd.Parameters.AddWithValue("@birthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@notes", data.Notes);
                cmd.Parameters.AddWithValue("@photo", data.Photo);
                cmd.Parameters.AddWithValue("@email", data.Email);
               

                result = Convert.ToInt32(cmd.ExecuteScalar());


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Đếm nhân viên dựa vào kết quả tìm kiếm
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
                                    from    Employees
                                    where  (@searchValue = N'')
                                        or (
                                                (LastName like @searchValue)
                                                or
                                                (FirstName like @searchValue)
                                                or
                                                (Email like @searchValue)
                                                or
                                                (Notes like @searchValue)
                                                or
                                                (YEAR(BirthDate) like @searchValue)
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
        /// Xóa nhân viên dựa vào mã nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public bool Delete(int employeeID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DELETE FROM Employees WHERE EmployeeID = @employeeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@employeeID", employeeID);

                result = cmd.ExecuteNonQuery() > 0;


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Lấy thông tin nhân viên dựa vào mã nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public Employee Get(int employeeID)
        {
            Employee data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Employees WHERE EmployeeID = @employeeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@employeeID", employeeID);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    data = new Employee()
                    {
                        EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                        LastName = Convert.ToString(dbReader["LastName"]),
                        FirstName = Convert.ToString(dbReader["FirstName"]),
                        BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                        Photo = Convert.ToString(dbReader["Photo"]),
                        Notes = Convert.ToString(dbReader["Notes"]),
                        Email = Convert.ToString(dbReader["Email"]),
                        Password = Convert.ToString(dbReader["Password"])
                    };
                }

                cn.Close();
            }

            return data;
        }
        /// <summary>
        /// Kiểm tra sự phụ thuộc của nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public bool InUsed(int employeeID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT COUNT(*)
                                    FROM Employees as e
	                                    RIGHT JOIN Orders as o ON e.EmployeeID = o.EmployeeID
                                    WHERE e.EmployeeID = @employeeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@employeeID", employeeID);


                result = (Convert.ToInt32(cmd.ExecuteScalar()) > 0);


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Lấy ra danh sách nhân viên
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public IList<Employee> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Employee> data = new List<Employee>();

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
                                                    row_number() over(order by LastName) as RowNumber
                                            from    Employees
                                            where    (@searchValue = N'')
                                                or (
                                                        (LastName like @searchValue)
                                                        or
                                                        (FirstName like @searchValue)
                                                        or
                                                        (Email like @searchValue)
                                                        or
                                                        (Notes like @searchValue)
                                                        
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
                    data.Add(new Employee()
                    {
                        EmployeeID = Convert.ToInt32(result["EmployeeID"]),
                        LastName = Convert.ToString(result["LastName"]),
                        FirstName = Convert.ToString(result["FirstName"]),
                        BirthDate = Convert.ToDateTime(result["BirthDate"]),
                        Photo = Convert.ToString(result["Photo"]),
                        Notes = Convert.ToString(result["Notes"]),
                        Email = Convert.ToString(result["Email"]),
                        Password = Convert.ToString(result["Password"])
                    });
                }

                cn.Close();
            }
            return data;
        }
        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Employee data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                    SET LastName = @lastName, 
	                                    FirstName = @firstName, 
	                                    BirthDate = @birthday, 
	                                    Photo = @photo,
	                                    Notes = @notes,
	                                    Email = @email
                                    WHERE EmployeeID = @employeeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@lastName", data.LastName);
                cmd.Parameters.AddWithValue("@firstName", data.FirstName);
                cmd.Parameters.AddWithValue("@birthday", data.BirthDate);
                cmd.Parameters.AddWithValue("@photo", data.Photo);
                cmd.Parameters.AddWithValue("@notes", data.Notes);
                cmd.Parameters.AddWithValue("@email", data.Email);
                //cmd.Parameters.AddWithValue("@password", data.Password);
                cmd.Parameters.AddWithValue("@employeeID", data.EmployeeID);


                result = cmd.ExecuteNonQuery() > 0;


                cn.Close();
            }
            return result;
        }
    }
}
